// Leaflet Map Initialization for Dashboard
(function() {
    'use strict';

    // Default Davao City coordinates
    const DAVAO_LAT = 7.0731;
    const DAVAO_LNG = 125.6121;
    const DEFAULT_ZOOM = 12;

    // Status colors
    const STATUS_COLORS = {
        normal: '#16a34a',   // green
        warning: '#d97706',  // yellow
        danger: '#dc2626'    // red
    };

    // Initialize map when DOM is ready
    function initializeMap() {
        const mapContainer = document.getElementById('floodMap');
        if (!mapContainer) {
            console.warn('Map container not found');
            return;
        }

        // Create map instance
        const map = L.map(mapContainer).setView([DAVAO_LAT, DAVAO_LNG], DEFAULT_ZOOM);

        // Add tile layer (OpenStreetMap)
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
            maxZoom: 19,
            opacity: 0.8
        }).addTo(map);

        // Add custom controls
        addMapControls(map);

        // Render sensor markers
        if (window.__sensorData && Array.isArray(window.__sensorData)) {
            renderSensorMarkers(map, window.__sensorData);
        }

        // Handle traffic layer toggle
        const trafficToggle = document.querySelector('.traffic-toggle input[type="checkbox"]');
        if (trafficToggle) {
            let satelliteLayer = null;

            trafficToggle.addEventListener('change', function() {
                if (this.checked) {
                    // Switch to satellite/traffic view (using CartoDB Voyager)
                    if (satelliteLayer) {
                        map.removeLayer(satelliteLayer);
                    }
                    satelliteLayer = L.tileLayer('https://{s}.tile-cyclosm.openstreetmap.fr/cyclosm/{z}/{x}/{y}.png', {
                        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>',
                        maxZoom: 20
                    }).addTo(map);
                } else {
                    // Switch back to standard OpenStreetMap
                    if (satelliteLayer) {
                        map.removeLayer(satelliteLayer);
                    }
                }
            });
        }

        return map;
    }

    // Add custom map controls (zoom, fullscreen, etc.)
    function addMapControls(map) {
        const controlsContainer = document.querySelector('.map-controls');
        if (!controlsContainer) return;

        // Fullscreen button
        const fullscreenBtn = controlsContainer.querySelector('[aria-label="Fullscreen"]');
        if (fullscreenBtn) {
            fullscreenBtn.addEventListener('click', function() {
                const mapContainer = document.getElementById('floodMap');
                if (mapContainer.requestFullscreen) {
                    mapContainer.requestFullscreen();
                }
            });
        }

        // Zoom in button
        const zoomInBtn = controlsContainer.querySelector('[aria-label="Zoom in"]');
        if (zoomInBtn) {
            zoomInBtn.addEventListener('click', function() {
                map.zoomIn();
            });
        }

        // Zoom out button
        const zoomOutBtn = controlsContainer.querySelector('[aria-label="Zoom out"]');
        if (zoomOutBtn) {
            zoomOutBtn.addEventListener('click', function() {
                map.zoomOut();
            });
        }
    }

    // Render sensor markers on the map
    function renderSensorMarkers(map, sensors) {
        sensors.forEach(sensor => {
            // Convert percentage coordinates to actual lat/lng if needed
            let lat = sensor.lat;
            let lng = sensor.lng;

            // If coordinates are percentages (0-100), convert them
            if (lat > 0 && lat < 100 && lng > 0 && lng < 100) {
                // Simple conversion: treat percentages as offsets from Davao City center
                // This is a placeholder - adjust based on your actual coordinate system
                lat = DAVAO_LAT + (lat - 50) * 0.01;
                lng = DAVAO_LNG + (lng - 50) * 0.01;
            }

            // Create custom HTML for marker
            const markerHtml = `
                <div class="leaflet-marker-custom status-${sensor.status}">
                    <i class="fa-solid fa-water"></i>
                </div>
            `;

            // Create custom icon
            const customIcon = L.divIcon({
                html: markerHtml,
                className: 'leaflet-div-icon-sensor',
                iconSize: [40, 40],
                iconAnchor: [20, 40],
                popupAnchor: [0, -40]
            });

            // Create popup content
            const popupContent = `
                <div class="sensor-popup">
                    <h4>${sensor.code}</h4>
                    <p><strong>Location:</strong> ${sensor.location}</p>
                    <p><strong>Water Level:</strong> ${sensor.waterLevelCm} cm</p>
                    <p><strong>Status:</strong> <span class="status-${sensor.status}">${sensor.status.toUpperCase()}</span></p>
                    <p><strong>Connection:</strong> ${sensor.online ? '<span class="online">Online</span>' : '<span class="offline">Offline</span>'}</p>
                </div>
            `;

            // Create marker
            const marker = L.marker([lat, lng], { icon: customIcon })
                .bindPopup(popupContent, {
                    maxWidth: 250,
                    className: 'sensor-popup-container'
                })
                .addTo(map);

            // Add tooltip on hover
            marker.bindTooltip(`${sensor.code} - ${sensor.location}`, {
                permanent: false,
                direction: 'top',
                offset: [0, -10]
            });
        });
    }

    // Initialize when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initializeMap);
    } else {
        initializeMap();
    }
})();
