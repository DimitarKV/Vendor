let map;
let marker;

export async function initMap(lat, lng, title) {
    let { Map } = await google.maps.importLibrary("maps");
    const {Marker} = await google.maps.importLibrary("marker")

    const position = { lat: lat, lng: lng };
    
    map = new Map(document.getElementById("map"), {
        center: { lat: lat, lng: lng },
        zoom: 15,
    })

    marker = new Marker({
        map: map,
        position: position,
        title: title,
    });

    const directionsControlDiv = document.createElement("div");
    const directionsControl = createGetDirectionsControl();
    directionsControlDiv.appendChild(directionsControl);
    map.controls[google.maps.ControlPosition.LEFT_BOTTOM].push(directionsControlDiv);
}

export async function getDirectionsInApp() {
    const url = `https://www.google.com/maps/dir/?api=1&destination=${marker.getPosition().lat()},${marker.getPosition().lng()}&dir_action=navigate`;
    window.location.href = url;
}

function createGetDirectionsControl() {
    const controlButton = document.createElement("button");

    // Set CSS for the control.
    controlButton.style.backgroundColor = "#1a73e8";
    controlButton.style.border = "2px solid #1a73e8";
    controlButton.style.borderRadius = "3px";
    controlButton.style.boxShadow = "0 2px 6px rgba(0,0,0,.3)";
    controlButton.style.color = "rgb(255,255,255)";
    controlButton.style.cursor = "pointer";
    controlButton.style.fontFamily = "Roboto,Arial,sans-serif";
    controlButton.style.fontSize = "16px";
    controlButton.style.lineHeight = "38px";
    controlButton.style.margin = "0 0 0 22px";
    controlButton.style.padding = "0 5px";
    controlButton.style.textAlign = "center";

    controlButton.textContent = "Directions";
    controlButton.title = "Click to get directions in the app";
    controlButton.type = "button";

    // Setup the click event listeners: simply set the map to Chicago.
    controlButton.addEventListener("click", () => {
        getDirectionsInApp();
    });

    return controlButton;
}




// initMap(42.128247, 24.728230);