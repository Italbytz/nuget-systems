SHELL := /bin/bash
.DEFAULT_GOAL := help

# ------------------------------------------------------------------------------
# Configuration & Variables
# ------------------------------------------------------------------------------
SOLUTION            := nuget-systems.sln
CONFIGURATION      ?= Debug
PACK_CONFIGURATION ?= Release

DOCFX_CONFIG        := docfx/docfx.json
DOCFX_OUT_DIR       := docfx/_site

DEMO_PROJECT        := samples/Italbytz.Systems.Demos.Web/Italbytz.Systems.Demos.Web.csproj
DEMO_PORT           ?= 5036
DEMO_HTTP_URL       := http://localhost:$(DEMO_PORT)
DEMO_BASE_HREF      := /nuget-systems/demos/

ARTIFACTS_DIR       := artifacts
DEMO_PUBLISH_DIR    := $(ARTIFACTS_DIR)/demo-build
PACKAGES_DIR        := $(ARTIFACTS_DIR)/packages
PAGES_DIR           := $(ARTIFACTS_DIR)/pages

PAGES_PORT         ?= 8080
PAGES_HTTP_URL      := http://localhost:$(PAGES_PORT)
PAGES_LOG_FILE      := /tmp/nuget-systems-pages-serve.log

# ------------------------------------------------------------------------------
# Phony Targets
# ------------------------------------------------------------------------------
.PHONY: help restore build test pack docs \
        demo-run demo-watch demo-open demo-stop demo-publish clean-demo \
        pages-prepare pages-serve pages-open pages-serve-open \
        feedback clean

# ------------------------------------------------------------------------------
# Help Target
# ------------------------------------------------------------------------------
help:
	@echo "Available targets:"
	@echo ""
	@echo "  Build & Packaging:"
	@echo "    make restore          - Restore solution and .NET tools"
	@echo "    make build            - Build the full solution ($(CONFIGURATION))"
	@echo "    make test             - Run the full test suite ($(CONFIGURATION))"
	@echo "    make pack             - Pack NuGet packages ($(PACK_CONFIGURATION))"
	@echo "    make clean            - Remove build artifacts and clean solution"
	@echo ""
	@echo "  Documentation & GitHub Pages:"
	@echo "    make docs             - Build the DocFX site"
	@echo "    make pages-prepare    - Build the combined Pages artifact (Docs + Demo)"
	@echo "    make pages-serve      - Serve Pages locally on $(PAGES_HTTP_URL)"
	@echo "    make pages-open       - Open Pages preview in browser"
	@echo "    make pages-serve-open - Start Pages server in background & open browser"
	@echo ""
	@echo "  Demo Application:"
	@echo "    make demo-run         - Run demo host on $(DEMO_HTTP_URL)"
	@echo "    make demo-watch       - Run demo host with dotnet watch on $(DEMO_HTTP_URL)"
	@echo "    make demo-open        - Open demo host in browser"
	@echo "    make demo-stop        - Stop running demo host process on port $(DEMO_PORT)"
	@echo "    make clean-demo       - Clean demo host bin/obj cache"
	@echo "    make demo-publish     - Publish demo host into $(DEMO_PUBLISH_DIR)"
	@echo ""
	@echo "  Workflow:"
	@echo "    make feedback         - Fast local feedback loop: prepare combined Pages artifact"

# ------------------------------------------------------------------------------
# Core Build & Test Targets
# ------------------------------------------------------------------------------
restore:
	dotnet tool restore
	dotnet restore $(SOLUTION)

build: restore
	dotnet build $(SOLUTION) --configuration $(CONFIGURATION) --no-restore

test: restore
	dotnet test $(SOLUTION) --configuration $(CONFIGURATION) --no-restore --verbosity minimal

pack: restore
	dotnet pack $(SOLUTION) --configuration $(PACK_CONFIGURATION) --no-restore --verbosity minimal --output $(PACKAGES_DIR)

# ------------------------------------------------------------------------------
# Documentation
# ------------------------------------------------------------------------------
docs:
	dotnet tool restore
	dotnet tool run docfx $(DOCFX_CONFIG)

# ------------------------------------------------------------------------------
# Demo Host
# ------------------------------------------------------------------------------
demo-run:
	dotnet run --project $(DEMO_PROJECT) --launch-profile http

demo-watch: demo-stop clean-demo
	dotnet watch --project $(DEMO_PROJECT) run --launch-profile http

demo-open:
	open $(DEMO_HTTP_URL)

demo-stop:
	-lsof -ti tcp:$(DEMO_PORT) | xargs kill 2>/dev/null || true

clean-demo:
	rm -rf samples/Italbytz.Systems.Demos.Web/bin samples/Italbytz.Systems.Demos.Web/obj

demo-publish:
	dotnet publish $(DEMO_PROJECT) --configuration $(PACK_CONFIGURATION) --output $(DEMO_PUBLISH_DIR)

# ------------------------------------------------------------------------------
# Pages Artifact Assembly & Preview
# ------------------------------------------------------------------------------
pages-prepare: docs demo-publish
	rm -rf $(PAGES_DIR)
	mkdir -p $(PAGES_DIR)/demos
	perl -0pi -e 's#<base href="/" />#<base href="$(DEMO_BASE_HREF)" />#' $(DEMO_PUBLISH_DIR)/wwwroot/index.html
	cp $(DEMO_PUBLISH_DIR)/wwwroot/index.html $(DEMO_PUBLISH_DIR)/wwwroot/404.html
	cp -R $(DOCFX_OUT_DIR)/. $(PAGES_DIR)
	cp -R $(DEMO_PUBLISH_DIR)/wwwroot/. $(PAGES_DIR)/demos
	cp $(DEMO_PUBLISH_DIR)/wwwroot/index.html $(PAGES_DIR)/404.html
	@for route in binary-conversion number-conversion binary-arithmetic binary-addition twos-complement decimal-to-binary dezimal-zu-binaer binary-to-decimal binaer-zu-dezimal computing-systems operating-systems networking cpu-simulator logic-simulator logic-circuits napier-bones normal-forms quine-mccluskey line-coding bitencoding crc subnetting cpu-scheduling realtime-scheduling buddy-memory page-replacement; do \
		mkdir -p $(PAGES_DIR)/demos/$$route; \
		cp $(DEMO_PUBLISH_DIR)/wwwroot/index.html $(PAGES_DIR)/demos/$$route/index.html; \
	done
	touch $(PAGES_DIR)/.nojekyll
	@echo "Pages artifact ready in $(PAGES_DIR)"

pages-serve: pages-prepare
	cd $(PAGES_DIR) && python3 -m http.server $(PAGES_PORT)

pages-open:
	open $(PAGES_HTTP_URL)

pages-serve-open: pages-prepare
	cd $(PAGES_DIR) && nohup python3 -m http.server $(PAGES_PORT) >$(PAGES_LOG_FILE) 2>&1 &
	@sleep 1
	open $(PAGES_HTTP_URL)
	@echo "Pages preview started in the background on $(PAGES_HTTP_URL)"
	@echo "Log: $(PAGES_LOG_FILE)"

# ------------------------------------------------------------------------------
# Feedback & Clean
# ------------------------------------------------------------------------------
feedback: pages-prepare
	@echo "Demo host:     $(DEMO_HTTP_URL) via 'make demo-watch' and 'make demo-open'"
	@echo "Pages preview: $(PAGES_HTTP_URL) via 'make pages-serve', 'make pages-open', or 'make pages-serve-open'"

clean:
	$(MAKE) clean-demo
	rm -rf $(DEMO_PUBLISH_DIR) $(PAGES_DIR) $(PACKAGES_DIR) $(DOCFX_OUT_DIR)
	dotnet clean $(SOLUTION)
