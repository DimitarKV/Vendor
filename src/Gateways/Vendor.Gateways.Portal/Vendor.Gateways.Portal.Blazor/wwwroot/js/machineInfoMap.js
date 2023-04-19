async function initMap(lat, lng) {
    var latlng = new google.maps.LatLng(lat, lng);
    var options = {
        zoom: 14, center: latlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    var map = new google.maps.Map(document.getElementById("map"), options);
}