// =========================================================
// DriveWise Dashboard — client behavior
// =========================================================
(function () {
    "use strict";

    // ---------- Live clock ----------
    function tick() {
        var now = new Date();
        var timeEl = document.getElementById("clockTime");
        var dateEl = document.getElementById("clockDate");
        if (timeEl) {
            timeEl.textContent = now.toLocaleTimeString([], { hour: "2-digit", minute: "2-digit", second: "2-digit" });
        }
        if (dateEl) {
            dateEl.textContent = now.toLocaleDateString([], { day: "2-digit", month: "short", year: "numeric", weekday: "long" });
        }
    }
    tick();
    setInterval(tick, 1000);

    // ---------- Mobile sidebar toggle ----------
    var toggleBtn = document.getElementById("sidebarToggle");
    var sidebar = document.querySelector(".sidebar");
    if (toggleBtn && sidebar) {
        toggleBtn.addEventListener("click", function () {
            sidebar.classList.toggle("open");
        });
    }

    // ---------- Water level chart ----------
    var canvas = document.getElementById("waterLevelChart");
    if (canvas && window.Chart) {
        var labels = window.__waterLevelLabels || [];
        var data = window.__waterLevelData || [];
        var warningThreshold = window.__warningThreshold || 70;
        var normalThreshold = window.__normalThreshold || 30;

        var ctx = canvas.getContext("2d");
        var gradient = ctx.createLinearGradient(0, 0, 0, 220);
        gradient.addColorStop(0, "rgba(220, 38, 38, 0.25)");
        gradient.addColorStop(0.5, "rgba(217, 119, 6, 0.18)");
        gradient.addColorStop(1, "rgba(22, 163, 74, 0.12)");

        // Per-point color depending on which zone the value falls in
        var pointColors = data.map(function (v) {
            if (v > warningThreshold) return "#dc2626";
            if (v > normalThreshold) return "#d97706";
            return "#16a34a";
        });

        new Chart(ctx, {
            type: "line",
            data: {
                labels: labels,
                datasets: [
                    {
                        label: "Water Level (cm)",
                        data: data,
                        borderColor: "#dc2626",
                        backgroundColor: gradient,
                        pointBackgroundColor: pointColors,
                        pointBorderColor: "#fff",
                        pointRadius: 5,
                        pointHoverRadius: 7,
                        borderWidth: 2.5,
                        tension: 0.35,
                        fill: true
                    }
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { display: false },
                    tooltip: {
                        callbacks: {
                            label: function (item) { return item.parsed.y + " cm"; }
                        }
                    }
                },
                scales: {
                    y: {
                        min: 0,
                        max: 100,
                        ticks: { stepSize: 25, color: "#7a8190", font: { size: 11 } },
                        grid: { color: "#eef0f3" }
                    },
                    x: {
                        ticks: { color: "#7a8190", font: { size: 11 } },
                        grid: { display: false }
                    }
                }
            }
        });
    }

    // ---------- Optional: poll the server for fresh sensor data ----------
    // Uncomment to auto-refresh stat cards / map pins every 30s without reload.
    /*
    function refreshDashboard() {
        fetch("/Dashboard/Refresh")
            .then(function (res) { return res.json(); })
            .then(function (json) {
                // Update DOM nodes here using the returned json
                // e.g. document.querySelector('.stat-value').textContent = json.totalSensors;
            })
            .catch(function (err) { console.error("Dashboard refresh failed:", err); });
    }
    setInterval(refreshDashboard, 30000);
    */
})();
