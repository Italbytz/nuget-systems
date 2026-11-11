using System;
using Italbytz.Exam.Trivia.Abstractions;

namespace Italbytz.Systems.Blazor.Components.Common;

public static class SystemsQuizCatalog
{
    private sealed class QuizYesNoQuestion : IYesNoQuestion
    {
        public bool Answer { get; set; }
        public string Category { get; set; } = string.Empty;
        public QuestionType QuestionType { get; set; } = QuestionType.Single;
        public Choices ChoicesType { get; set; } = Choices.Boolean;
        public Difficulty Difficulty { get; set; } = Difficulty.Medium;
        public string Text { get; set; } = string.Empty;
        public IQuestion AlternativeQuestion { get; set; } = null!;
    }

    public static IYesNoQuestion[] OperatingSystemsQuestions { get; } =
    [
        // 0..16: Original Catalog
        new QuizYesNoQuestion { Text = "Die MMU übersetzt beim Paging logische Speicheradressen mit der Seitentabelle in physische Adressen.", Answer = true, Category = "Speicherverwaltung" },
        new QuizYesNoQuestion { Text = "Beim Paging haben alle Seiten die gleiche Länge.", Answer = true, Category = "Speicherverwaltung" },
        new QuizYesNoQuestion { Text = "Ein Nachteil kurzer Seiten beim Paging ist, dass die Seitentabelle sehr groß werden kann.", Answer = true, Category = "Speicherverwaltung" },
        new QuizYesNoQuestion { Text = "Journaling-Dateisysteme grenzen die bei der Konsistenzprüfung zu überprüfenden Daten ein.", Answer = true, Category = "Dateisysteme" },
        new QuizYesNoQuestion { Text = "Bei Dateisystemen mit Journal sind Datenverluste garantiert ausgeschlossen.", Answer = false, Category = "Dateisysteme" },
        new QuizYesNoQuestion { Text = "Ein Journal im Dateisystem reduziert die Anzahl der Schreibzugriffe.", Answer = false, Category = "Dateisysteme" },
        new QuizYesNoQuestion { Text = "Der Zugriff auf Festplatten erfolgt blockorientiert und kann wahlfrei geschehen.", Answer = true, Category = "Dateisysteme" },
        new QuizYesNoQuestion
        {
            Text = "Round Robin kann als faires Schedulingverfahren bezeichnet werden.",
            Answer = true,
            Category = "Prozessverwaltung",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Prioritätengesteuertes Scheduling kann als faires Schedulingverfahren bezeichnet werden.",
                Answer = false,
                Category = "Prozessverwaltung"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Mit Prioritätengesteuertem Scheduling lässt sich die geringste Gesamtlaufzeit für Aufgaben erreichen.",
            Answer = false,
            Category = "Prozessverwaltung",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Mit Stapelbetrieb lässt sich die geringste Gesamtlaufzeit für Aufgaben erreichen.",
                Answer = true,
                Category = "Prozessverwaltung"
            }
        },
        new QuizYesNoQuestion { Text = "Shortest Job First ist ein in der Praxis eingesetztes Schedulingverfahren.", Answer = false, Category = "Prozessverwaltung" },
        new QuizYesNoQuestion { Text = "Shortest Job First garantiert die geringste durchschnittliche Laufzeit.", Answer = true, Category = "Prozessverwaltung" },
        new QuizYesNoQuestion { Text = "Monolithische Betriebssystemkerne sind leichter zu erweitern und zu warten.", Answer = false, Category = "Grundlagen" },
        new QuizYesNoQuestion { Text = "Mikrokernel führen zu einem langsameren Betriebssystem.", Answer = true, Category = "Grundlagen" },
        new QuizYesNoQuestion { Text = "Aktives Warten vor dem Betreten eines kritischen Abschnitts verschwendet Rechenzeit.", Answer = true, Category = "Synchronisation" },
        new QuizYesNoQuestion { Text = "Defragmentieren ist auch bei SSDs nötig.", Answer = false, Category = "Dateisysteme" },
        new QuizYesNoQuestion { Text = "Eine Vergrößerung des Speichers kann in manchen Ersetzungsstrategien zu mehr Seitenfehlern führen.", Answer = true, Category = "Speicherverwaltung" },
        new QuizYesNoQuestion
        {
            Text = "Die Funktion eines Mutex lässt sich auch mit Semaphoren erreichen.",
            Answer = true,
            Category = "Synchronisation",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Die Funktion einer Semaphore lässt sich auch mit einem Mutex erreichen.",
                Answer = false,
                Category = "Synchronisation"
            }
        },
        // 17..34: Extended Catalog
        new QuizYesNoQuestion
        {
            Text = "Ein Trap (Software-Interrupt) wird synchron durch das aktuell ausgeführte Programm ausgelöst.",
            Answer = true,
            Category = "Rechnerarchitektur",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Ein Hardware-Interrupt wird synchron durch das aktuell ausgeführte Programm ausgelöst.",
                Answer = false,
                Category = "Rechnerarchitektur"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Im Kernel Mode (Privileged Mode) darf die CPU alle Maschinenbefehle einschließlich privilegierter Instruktionen ausführen.",
            Answer = true,
            Category = "Rechnerarchitektur",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Im User Mode darf die CPU alle Maschinenbefehle einschließlich privilegierter Instruktionen ausführen.",
                Answer = false,
                Category = "Rechnerarchitektur"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Der Systemaufruf fork() erzeugt einen neuen Kindprozess als Duplikat des Elternprozesses mit eigenem Adressraum.",
            Answer = true,
            Category = "Systemaufrufe",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Der Systemaufruf fork() ersetzt das Speicherabbild des Prozesses durch ein neues Programm.",
                Answer = false,
                Category = "Systemaufrufe"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Der Systemaufruf fork() gibt im neu erzeugten Kindprozess den Rückgabewert 0 zurück.",
            Answer = true,
            Category = "Systemaufrufe",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Der Systemaufruf fork() gibt im aufrufenden Elternprozess den Rückgabewert 0 zurück.",
                Answer = false,
                Category = "Systemaufrufe"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Ein Prozess im Zustand Running wechselt nach Ready, wenn seine zugewiesene Zeitscheibe abgelaufen ist.",
            Answer = true,
            Category = "Prozessverwaltung",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Ein Prozess im Zustand Running wechselt nach Blocked, wenn seine zugewiesene Zeitscheibe abgelaufen ist.",
                Answer = false,
                Category = "Prozessverwaltung"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Threads desselben Prozesses teilen sich denselben virtuellen Adressraum und globale Variablen.",
            Answer = true,
            Category = "Prozessverwaltung",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Threads desselben Prozesses besitzen jeweils vollständig voneinander getrennte und isolierte Adressräume.",
                Answer = false,
                Category = "Prozessverwaltung"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "I/O-lastige Prozesse verbringen den Großteil ihrer Zeit mit dem Warten auf Ein-/Ausgabe und besitzen kurze CPU-Bursts.",
            Answer = true,
            Category = "Prozessverwaltung",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "CPU-lastige Prozesse verbringen den Großteil ihrer Zeit mit dem Warten auf Ein-/Ausgabeoperationen.",
                Answer = false,
                Category = "Prozessverwaltung"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Der Completely Fair Scheduler (CFS) in Linux wählt zur Ausführung stets den Thread mit der geringsten virtual runtime (vruntime).",
            Answer = true,
            Category = "Prozessverwaltung",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Der Completely Fair Scheduler (CFS) in Linux teilt Prozesse rein nach festen statischen Prioritäten ohne Berücksichtigung der Laufzeit ein.",
                Answer = false,
                Category = "Prozessverwaltung"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Eine Race Condition tritt auf, wenn mehrere Threads ohne Synchronisation nebenläufig auf geteilte Daten zugreifen und das Ergebnis vom zeitlichen Ablauf abhängt.",
            Answer = true,
            Category = "Synchronisation",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Eine Race Condition tritt nur auf, wenn Prozesse auf unterschiedlichen physischen Rechnern über das Netzwerk kommunizieren.",
                Answer = false,
                Category = "Synchronisation"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Der Peterson-Algorithmus garantiert wechselseitigen Ausschluss für genau zwei Prozesse ohne spezielle Hardware-Instruktionen.",
            Answer = true,
            Category = "Synchronisation",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Der Peterson-Algorithmus eignet sich ohne Erweiterung für eine beliebige Anzahl von N Prozessen.",
                Answer = false,
                Category = "Synchronisation"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Eine Standard-UNIX-Pipe überträgt Daten unidirektional als Bytestrom nach dem FIFO-Prinzip.",
            Answer = true,
            Category = "Interprozesskommunikation",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Eine Standard-UNIX-Pipe überträgt Daten bidirektional und paketorientiert mit wahlfreiem Zugriff.",
                Answer = false,
                Category = "Interprozesskommunikation"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Der Translation Lookaside Buffer (TLB) ist ein schneller Cache in der MMU für kürzlich genutzte logisch-zu-physisch Adressübersetzungen.",
            Answer = true,
            Category = "Speicherverwaltung",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Der Translation Lookaside Buffer (TLB) speichert ausgelagerte Speicherseiten auf der Festplatte zwischen.",
                Answer = false,
                Category = "Speicherverwaltung"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Paging verhindert externe Fragmentierung vollständig, kann jedoch interne Fragmentierung auf der letzten Seite verursachen.",
            Answer = true,
            Category = "Speicherverwaltung",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Paging verhindert interne Fragmentierung vollständig, führt jedoch zu starker externer Fragmentierung.",
                Answer = false,
                Category = "Speicherverwaltung"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Thrashing (Seitenflattern) bezeichnet den Zustand, bei dem das System fast ausschließlich mit dem Ein- und Auslagern von Seiten ausgelastet ist.",
            Answer = true,
            Category = "Speicherverwaltung",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Thrashing bezeichnet das geordnete Defragmentieren der Auslagerungsdatei im laufenden Betrieb.",
                Answer = false,
                Category = "Speicherverwaltung"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "In UNIX-Dateisystemen speichert der Inode Metadaten und Blockzeiger, enthält jedoch nicht den Namen der Datei.",
            Answer = true,
            Category = "Dateisysteme",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "In UNIX-Dateisystemen wird der Dateiname direkt als Attribut im Inode der Datei gespeichert.",
                Answer = false,
                Category = "Dateisysteme"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Ein Hard Link verweist direkt auf denselben Inode wie die Originaldatei und bleibt auch nach Löschen des ursprünglichen Dateinamens lesbar.",
            Answer = true,
            Category = "Dateisysteme",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Ein Hard Link speichert lediglich den relativen Pfadnamen zur Zieldatei in einem eigenen Inode.",
                Answer = false,
                Category = "Dateisysteme"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Ein Typ-1-Hypervisor (Bare-Metal) läuft direkt auf der physischen Hardware ohne zwischengeschaltetes Host-Betriebssystem.",
            Answer = true,
            Category = "Virtualisierung",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Ein Typ-2-Hypervisor (Hosted) läuft direkt auf der physischen Hardware ohne zwischengeschaltetes Host-Betriebssystem.",
                Answer = false,
                Category = "Virtualisierung"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Container teilen sich den Betriebssystemkern des Hosts und isolieren Prozesse über Linux Namespaces und cgroups.",
            Answer = true,
            Category = "Virtualisierung",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Jeder Container führt einen vollständig eigenen, unabhängigen Betriebssystemkern aus.",
                Answer = false,
                Category = "Virtualisierung"
            }
        }
    ];

    public static IYesNoQuestion[] NetworkingQuestions { get; } =
    [
        // 0..13: Original Catalog
        new QuizYesNoQuestion { Text = "Die Mindestlänge von Rahmen dient dazu, Kollisionen erkennen zu können.", Answer = true, Category = "Sicherungsschicht" },
        new QuizYesNoQuestion { Text = "Die Mindestlänge von Rahmen reicht um Kollisionen zu vermeiden.", Answer = false, Category = "Sicherungsschicht" },
        new QuizYesNoQuestion
        {
            Text = "Bei bekannter IP-Adresse eines Servers können wir mit dessen Prozessen kommunizieren.",
            Answer = false,
            Category = "Transportschicht",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Bei bekannter IP-Adresse eines Webservers können wir Webseiten anfragen.",
                Answer = true,
                Category = "Transportschicht"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "UDP ist für Videotelefonie besser geeignet als TCP.",
            Answer = true,
            Category = "Transportschicht",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "TCP ist für Videotelefonie besser geeignet als UDP.",
                Answer = false,
                Category = "Transportschicht"
            }
        },
        new QuizYesNoQuestion { Text = "Alle Netzwerkgeräte im gleichen physischen Netz empfangen Rahmen mit der Zieladresse FF-FF-FF-FF-FF-FF.", Answer = true, Category = "Sicherungsschicht" },
        new QuizYesNoQuestion
        {
            Text = "Die Netzwerktopologie Maschen enthält einen Single Point of Failure.",
            Answer = false,
            Category = "Grundlagen",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Die Netzwerktopologie Bus enthält einen Single Point of Failure.",
                Answer = true,
                Category = "Grundlagen"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Router untersuchen Rahmen mit Prüfsummen auf Korrektheit.",
            Answer = false,
            Category = "Sicherungsschicht",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Bridges untersuchen Rahmen mit Prüfsummen auf Korrektheit.",
                Answer = true,
                Category = "Sicherungsschicht"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Bridges vermeiden Kreise mit dem Spanning Tree Protocol.",
            Answer = true,
            Category = "Sicherungsschicht",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Router vermeiden Kreise mit dem Spanning Tree Protocol.",
                Answer = false,
                Category = "Sicherungsschicht"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Der Leitungscode Non-Return-To-Zero (NRZ) verursacht Probleme bei der Taktwiederherstellung.",
            Answer = true,
            Category = "Bitübertragungsschicht",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Der Leitungscode Manchester verursacht Probleme bei der Taktwiederherstellung.",
                Answer = false,
                Category = "Bitübertragungsschicht"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Gateways in der Vermittlungsschicht von Computernetzen werden häufig eingesetzt.",
            Answer = false,
            Category = "Vermittlungsschicht",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Gateways in der Vermittlungsschicht von Computernetzen sind heutzutage selten nötig.",
                Answer = true,
                Category = "Vermittlungsschicht"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Telnet ermöglicht die verschlüsselte Fernsteuerung von Computern.",
            Answer = false,
            Category = "Anwendungsschicht",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Telnet ermöglicht die unverschlüsselte Fernsteuerung von Computern.",
                Answer = true,
                Category = "Anwendungsschicht"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "DHCP ermöglicht die Zuweisung der Netzwerkkonfiguration an Netzwerkgeräte.",
            Answer = true,
            Category = "Vermittlungsschicht",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "ICMP ermöglicht die Zuweisung der Netzwerkkonfiguration an Netzwerkgeräte.",
                Answer = false,
                Category = "Vermittlungsschicht"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "ICMP dient dazu, Diagnose- und Fehlermeldungen auszutauschen.",
            Answer = true,
            Category = "Vermittlungsschicht",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "DHCP dient dazu, Diagnose- und Fehlermeldungen auszutauschen.",
                Answer = false,
                Category = "Vermittlungsschicht"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Im Ethernet-Rahmen stehen die MAC-Adressen von Sender und Empfänger.",
            Answer = true,
            Category = "Sicherungsschicht",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Im Ethernet-Rahmen stehen die IP-Adressen von Sender und Empfänger.",
                Answer = false,
                Category = "Sicherungsschicht"
            }
        },
        // 14..31: Extended Catalog
        new QuizYesNoQuestion
        {
            Text = "Im OSI-Referenzmodell ist die Transportschicht die Schicht 4 und stellt Ende-zu-Ende-Verbindungen bereit.",
            Answer = true,
            Category = "Grundlagen",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Im OSI-Referenzmodell ist die Vermittlungsschicht die Schicht 4 und stellt Ende-zu-Ende-Verbindungen bereit.",
                Answer = false,
                Category = "Grundlagen"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Die Protokolldateneinheit (PDU) der Sicherungsschicht (Schicht 2) wird als Rahmen (Frame) bezeichnet.",
            Answer = true,
            Category = "Grundlagen",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Die Protokolldateneinheit (PDU) der Sicherungsschicht (Schicht 2) wird als Paket (Packet) bezeichnet.",
                Answer = false,
                Category = "Grundlagen"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Bei Vollduplex-Übertragung können beide Stationen gleichzeitig Daten senden und empfangen.",
            Answer = true,
            Category = "Bitübertragungsschicht",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Bei Halbduplex-Übertragung können beide Stationen gleichzeitig Daten senden und empfangen.",
                Answer = false,
                Category = "Bitübertragungsschicht"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Lichtwellenleiter (Glasfaser) sind unempfindlich gegenüber äußeren elektromagnetischen Störfeldern.",
            Answer = true,
            Category = "Bitübertragungsschicht",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Lichtwellenleiter (Glasfaser) sind anfälliger für elektromagnetische Störungen als ungeschirmte Twisted-Pair-Kupferkabel.",
                Answer = false,
                Category = "Bitübertragungsschicht"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Ein Switch trennt Kollisionsdomänen und leitet Rahmen anhand ihrer Ziel-MAC-Adresse gezielt weiter.",
            Answer = true,
            Category = "Sicherungsschicht",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Ein Hub trennt Kollisionsdomänen und leitet Rahmen gezielt anhand der MAC-Adresse weiter.",
                Answer = false,
                Category = "Sicherungsschicht"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "CSMA/CD erkennt Kollisionen während des Sendens und wird im klassischen Halbduplex-Ethernet verwendet.",
            Answer = true,
            Category = "Sicherungsschicht",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "CSMA/CD vermeidet Kollisionen durch vorherige Reservierung und wird primär im WLAN verwendet.",
                Answer = false,
                Category = "Sicherungsschicht"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Das Address Resolution Protocol (ARP) ermittelt zu einer bekannten IP-Adresse die zugehörige MAC-Adresse im lokalen Netz.",
            Answer = true,
            Category = "Sicherungsschicht",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Das Address Resolution Protocol (ARP) ermittelt zu einer bekannten MAC-Adresse die weltweite öffentliche IP-Adresse.",
                Answer = false,
                Category = "Sicherungsschicht"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Eine IPv6-Adresse ist 128 Bit lang und wird üblicherweise in acht Blöcken hexadezimal notiert.",
            Answer = true,
            Category = "Vermittlungsschicht",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Eine IPv6-Adresse ist 64 Bit lang und wird in vier dezimalen Oktetten notiert.",
                Answer = false,
                Category = "Vermittlungsschicht"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Die Subnetzmaske trennt eine IP-Adresse bitweise in einen Netzanteil (Präfix) und einen Hostanteil auf.",
            Answer = true,
            Category = "Vermittlungsschicht",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Die Subnetzmaske gibt die maximale Anzahl an Zwischenstationen (Hops) an, die ein Paket durchlaufen darf.",
                Answer = false,
                Category = "Vermittlungsschicht"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Private IP-Adressbereiche (nach RFC 1918) werden im öffentlichen globalen Internet nicht geroutet.",
            Answer = true,
            Category = "Vermittlungsschicht",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Private IP-Adressen aus dem Bereich 192.168.0.0/16 werden im weltweiten Internet direkt und unmodifiziert geroutet.",
                Answer = false,
                Category = "Vermittlungsschicht"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Das Time-to-Live-Feld (TTL) im IPv4-Header wird von jedem weiterleitenden Router dekrementiert, um Endlosschleifen zu verhindern.",
            Answer = true,
            Category = "Vermittlungsschicht",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Das Time-to-Live-Feld (TTL) im IPv4-Header misst die verbleibende Gültigkeitsdauer einer aktiven TCP-Verbindung in Millisekunden.",
                Answer = false,
                Category = "Vermittlungsschicht"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Link-State-Routing-Protokolle (wie OSPF) basieren auf dem Dijkstra-Algorithmus und berechnen Pfade auf Basis der Gesamttopologie.",
            Answer = true,
            Category = "Vermittlungsschicht",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Distance-Vector-Routing-Protokolle (wie RIP) verteilen stets die vollständige globale Netzwerktopologie an jeden Router.",
                Answer = false,
                Category = "Vermittlungsschicht"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Traceroute ermittelt den Pfad zu einem Ziel, indem es Pakete mit sukzessive ansteigendem TTL-Wert (1, 2, 3...) versendet.",
            Answer = true,
            Category = "Diagnose",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Traceroute ermittelt den Pfad, indem es die Routing-Tabellen aller beteiligten Router über SNMP direkt ausliest.",
                Answer = false,
                Category = "Diagnose"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Die standardisierten Well-Known Ports (z. B. HTTP 80, SSH 22) liegen im Bereich von 0 bis 1023.",
            Answer = true,
            Category = "Transportschicht",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Die standardisierten Well-Known Ports liegen im dynamischen Bereich von 49152 bis 65535.",
                Answer = false,
                Category = "Transportschicht"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Der Drei-Wege-Handshake (Three-Way Handshake) von TCP erfolgt in der Reihenfolge: SYN -> SYN-ACK -> ACK.",
            Answer = true,
            Category = "Transportschicht",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Der Drei-Wege-Handshake von TCP erfolgt in der Reihenfolge: ACK -> SYN -> FIN.",
                Answer = false,
                Category = "Transportschicht"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Die Flusskontrolle bei TCP schützt den Empfänger vor Überlastung, während die Überlastkontrolle das Zwischennetz schützt.",
            Answer = true,
            Category = "Transportschicht",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Die Flusskontrolle bei TCP schützt primär die Zwischenrouter im Weitverkehrsnetz vor Pufferüberläufen.",
                Answer = false,
                Category = "Transportschicht"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "Das Domain Name System (DNS) löst Rechnernamen hierarchisch in IP-Adressen auf und nutzt standardmäßig Port 53.",
            Answer = true,
            Category = "Anwendungsschicht",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "Das Domain Name System (DNS) löst MAC-Adressen in IP-Adressen auf und nutzt standardmäßig Port 80.",
                Answer = false,
                Category = "Anwendungsschicht"
            }
        },
        new QuizYesNoQuestion
        {
            Text = "VLANs nach IEEE 802.1Q unterteilen ein physisches Netz auf Schicht 2 in voneinander getrennte Broadcast-Domänen.",
            Answer = true,
            Category = "Netzwerkvirtualisierung",
            AlternativeQuestion = new QuizYesNoQuestion
            {
                Text = "VLANs nach IEEE 802.1Q verbinden autonome Systeme im globalen Internet auf Schicht 3.",
                Answer = false,
                Category = "Netzwerkvirtualisierung"
            }
        }
    ];
}
