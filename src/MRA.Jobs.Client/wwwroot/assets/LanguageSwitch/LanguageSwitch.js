function change_period2(period) {
    var monthly = document.getElementById("monthly2");
    var semester = document.getElementById("semester2");
    var annual = document.getElementById("annual2");
    var selector = document.getElementById("selector");
    if (period === "English") {
        selector.style.left = 0;
        selector.style.width = monthly.clientWidth + "px";
        selector.style.backgroundColor = "#777777";
        selector.innerHTML = "English";
    } else if (period === "Русский") {
        selector.style.left = monthly.clientWidth + "px";
        selector.style.width = semester.clientWidth + "px";
        selector.innerHTML = "Русский";
        selector.style.backgroundColor = "#418d92";
    } else {
        selector.style.left = monthly.clientWidth + semester.clientWidth + 1 + "px";
        selector.style.width = annual.clientWidth + "px";
        selector.innerHTML = "Точики";
        selector.style.backgroundColor = "#4d7ea9";
    }
}

function InitLocation() {
    var location = localStorage.getItem('ApplicationCulturesNames');
    if (location === 'en-US') {
        change_period2('English');
    } else if (location === 'ru-RU') {
        change_period2('Русский');
    } else if (location === 'tj') {
        change_period2('Тоҷикӣ');
    }
}