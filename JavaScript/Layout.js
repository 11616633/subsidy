// Function to toggle between logos based on screen size
function toggleLogos() {
    var logoMini = document.getElementById("logoMini");
    var logoLarge = document.getElementById("logoLarge");
    if (window.innerWidth <= 768) { // Change the breakpoint as per your requirement
        logoMini.style.display = "inline-block";
        logoLarge.style.display = "none";
    } else {
        logoMini.style.display = "none";
        logoLarge.style.display = "inline-block";
    }
}

// Initial toggle
toggleLogos();

// Event listener for window resize
window.addEventListener("resize", toggleLogos);



$(document).ready(function () {
    $('.navbar-nav .nav-link').click(function () {
        $('body').toggleClass('sidebar-collapse');

    });
});