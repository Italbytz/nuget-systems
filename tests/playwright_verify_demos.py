#!/usr/bin/env python3
import http.server
import socketserver
import threading
import time
import os
import sys
from playwright.sync_api import sync_playwright

import socket

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.bind(("", 0))
    PORT = s.getsockname()[1]

WWWROOT = os.path.abspath("artifacts/demo-build/wwwroot")

class CustomHandler(http.server.SimpleHTTPRequestHandler):
    def __init__(self, *args, **kwargs):
        super().__init__(*args, directory=WWWROOT, **kwargs)

    def end_headers(self):
        self.send_header("Cache-Control", "no-cache, no-store, must-revalidate")
        self.send_header("Pragma", "no-cache")
        self.send_header("Expires", "0")
        super().end_headers()

    def do_GET(self):
        # Route rewrite for SPA routes
        url_path = self.path.split("?")[0]
        full_path = os.path.join(WWWROOT, url_path.lstrip("/"))
        if not os.path.exists(full_path) or os.path.isdir(full_path):
            self.path = "/index.html"
        return super().do_GET()

def run_server():
    socketserver.TCPServer.allow_reuse_address = True
    with socketserver.TCPServer(("", PORT), CustomHandler) as httpd:
        httpd.serve_forever()

server_thread = threading.Thread(target=run_server, daemon=True)
server_thread.start()
time.sleep(1)

CHROME_PATH = "/Applications/Google Chrome.app/Contents/MacOS/Google Chrome"

with sync_playwright() as p:
    browser = p.chromium.launch(executable_path=CHROME_PATH, headless=True)
    context = browser.new_context(bypass_csp=True, service_workers="block")
    page = context.new_page()

    print("=== Testing Walsh-Code Demo ===")
    page.goto(f"http://localhost:{PORT}/walsh-code?embed=true")
    page.wait_for_selector(".channel-chart-box", timeout=15000)

    # Verify SVG bars in .channel-chart-box have valid geometry
    bars = page.eval_on_selector_all(".channel-svg rect.svg-bar", """els => els.map(e => ({
        x: e.getAttribute('x'),
        y: e.getAttribute('y'),
        width: e.getAttribute('width'),
        height: e.getAttribute('height'),
        bboxHeight: e.getBBox().height,
        className: e.className.baseVal
    }))""")

    print(f"Found {len(bars)} SVG bars:")
    has_invalid_comma = False
    has_rendered_bars = False
    for i, b in enumerate(bars):
        print(f"  Bar {i}: x={b['x']}, y={b['y']}, h={b['height']}, bboxH={b['bboxHeight']}, cls={b['className']}")
        if "," in (b['height'] or "") or "," in (b['y'] or "") or "," in (b['x'] or ""):
            has_invalid_comma = True
        if b['bboxHeight'] > 2:
            has_rendered_bars = True

    assert not has_invalid_comma, "FAIL: SVG attributes contain German decimal commas!"
    assert has_rendered_bars, "FAIL: SVG bars have 0 rendered height!"
    print("SUCCESS: Walsh-Code SVG chart renders cleanly with valid dots and positive height!")

    print("\n=== Testing Binary-Arithmetic Demo ===")
    ba_context = browser.new_context(bypass_csp=True, service_workers="block")
    ba_page = ba_context.new_page()
    ba_page.on("console", lambda msg: print(f"[Browser Console {msg.type}] {msg.text}"))
    ba_page.on("pageerror", lambda err: print(f"[Browser PageError] {err}"))
    ba_page.goto(f"http://localhost:{PORT}/binary-arithmetic?embed=true")
    ba_page.wait_for_selector(".matrix-table", timeout=15000)
    ba_page.wait_for_timeout(1000)

    # Initial inputs: 13 + 29 = 42
    # Binary:
    # A = 0 0 0 0 1 1 0 1 (13)
    # B = 0 0 0 1 1 1 0 1 (29)
    # Carries:
    # Bit 0 produces carry 1 into Bit 1
    print("Initial Row 2:\n", ba_page.eval_on_selector(".matrix-table tbody tr:nth-child(3)", "e => e.outerHTML"))
    # Step through all steps
    for step in range(1, 9):
        ba_page.click("button[title='Nächster Schritt']")
        ba_page.wait_for_timeout(300)
        p_text = ba_page.inner_text(".player-progress")
        r_html = ba_page.eval_on_selector(".matrix-table tbody tr:nth-child(3)", "e => e.outerHTML")
        print(f"Step {step} ({p_text}):\n  {r_html}")

    rows = ba_page.eval_on_selector_all(".matrix-table tbody tr", """els => els.map((e, idx) => ({
        idx: idx,
        cls: e.className,
        label: e.querySelector("td") ? e.querySelector("td").innerText.trim() : "",
        cells: Array.from(e.querySelectorAll(".matrix-cell")).map(c => c.textContent.trim())
    }))""")
    for r in rows:
        print(f"Row {r['idx']} (label='{r['label']}'): cells={r['cells']}")
    print("Exact Row 2 outerHTML:\n", ba_page.eval_on_selector(".matrix-table tbody tr:nth-child(3)", "e => e.outerHTML"))
    carry_cells = rows[2]['cells']
    # 9 cells: 2^8, 2^7, 2^6, 2^5, 2^4, 2^3, 2^2, 2^1, 2^0
    # Expected carries for 13+29 at 2^5, 2^4, 2^3, 2^1 should be "1"
    # Indices:
    # 0: 2^8 -> ""
    # 1: 2^7 -> ""
    # 2: 2^6 -> ""
    # 3: 2^5 -> "1"
    # 4: 2^4 -> "1"
    # 5: 2^3 -> "1"
    # 6: 2^2 -> ""
    # 7: 2^1 -> "1"
    # 8: 2^0 -> ""
    assert carry_cells[3] == "1", f"Expected carry at 2^5, got '{carry_cells[3]}'"
    assert carry_cells[4] == "1", f"Expected carry at 2^4, got '{carry_cells[4]}'"
    assert carry_cells[5] == "1", f"Expected carry at 2^3, got '{carry_cells[5]}'"
    assert carry_cells[7] == "1", f"Expected carry at 2^1, got '{carry_cells[7]}'"
    print("SUCCESS: Simulator mode reveals all carries correctly!")

    # Test Worksheet Mode
    print("\n=== Testing Binary-Arithmetic Worksheet Mode ===")
    ba_page.click("button:has-text('Klausur-Arbeitsblatt')")
    ba_page.wait_for_selector(".worksheet-container", timeout=5000)

    # Click "Lösung anzeigen"
    ba_page.click("button:has-text('Lösung anzeigen')")
    ba_page.wait_for_timeout(300)

    ws_carry_cells = ba_page.eval_on_selector_all(
        ".worksheet-container .matrix-table tbody tr:nth-child(3) .matrix-cell",
        "els => els.map(e => e.innerText.trim())"
    )
    print("Worksheet carry cells with solution:", ws_carry_cells)
    assert ws_carry_cells[3] == "1", f"WS: Expected carry at 2^5, got '{ws_carry_cells[3]}'"
    assert ws_carry_cells[4] == "1", f"WS: Expected carry at 2^4, got '{ws_carry_cells[4]}'"
    assert ws_carry_cells[5] == "1", f"WS: Expected carry at 2^3, got '{ws_carry_cells[5]}'"
    assert ws_carry_cells[7] == "1", f"WS: Expected carry at 2^1, got '{ws_carry_cells[7]}'"
    print("SUCCESS: Worksheet mode solution shows all carries correctly!")

    # Test overflow addition (e.g. 200 + 100 = 300)
    print("\n=== Testing 9-Bit Overflow (200 + 100 = 300) ===")
    ba_page.click("button:has-text('Interaktiver Simulator')")
    inputs = ba_page.locator(".input-grid input")
    inputs.nth(0).fill("200")
    inputs.nth(0).dispatch_event("change")
    inputs.nth(1).fill("100")
    inputs.nth(1).dispatch_event("change")
    ba_page.wait_for_timeout(300)

    # Click Ende (Gesamtlösung anzeigen)
    ba_page.click("button[title='Gesamtlösung anzeigen']")
    ba_page.wait_for_timeout(300)

    overflow_carry_cells = ba_page.eval_on_selector_all(
        ".matrix-table tbody tr:nth-child(3) .matrix-cell",
        "els => els.map(e => e.innerText.trim())"
    )
    print("Overflow carry cells (200 + 100):", overflow_carry_cells)
    # 200 = 11001000, 100 = 01100100 -> Sum = 300 (100101100). Overflow carry at 2^8 must be "1"!
    assert overflow_carry_cells[0] == "1", f"Expected overflow carry at 2^8 to be '1', got '{overflow_carry_cells[0]}'"
    print("SUCCESS: 9-Bit overflow carry into 2^8 is correctly displayed!")

    browser.close()
    print("\nALL PLAYWRIGHT TESTS PASSED CLEANLY!")
