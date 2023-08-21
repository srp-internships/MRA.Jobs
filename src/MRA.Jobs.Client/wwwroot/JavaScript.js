
window.addEventListener('load', function () {
    var navLinks = document.querySelectorAll('.navmenu ul li a');
    navLinks.forEach(function (link) {
        link.addEventListener('click', function () {
            navLinks.forEach(function (link) {
                link.classList.remove('active');
            });
            link.classList.add('active');
        });
    });
});

