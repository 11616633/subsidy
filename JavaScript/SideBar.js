$(document).ready(function () {
    $('.nav-link[data-toggle="collapse"]').on('click', function (e) {
        e.preventDefault();
        e.stopPropagation();

        var $submenu = $(this).next('.collapse');
        var $otherSubmenus = $('.collapse').not($submenu);

        // Close other open dropdowns
        $otherSubmenus.removeClass('show');
        $('.nav-link').removeClass('active');

        // Toggle the visibility of the submenu
        if ($submenu.hasClass('show')) {
            e.preventDefault();
            $submenu.removeClass('show');
            $(this).removeClass('active');
        } else {
            e.preventDefault();
            $submenu.addClass('show');
            $(this).addClass('active');
        }
    });

    // Highlight the clicked item in red
    $('.nav-link:not([data-toggle="collapse"])').on('click', function () {
        $('.nav-link').removeClass('active');
        $(this).addClass('active');
    });

    // Change background color of li elements under ul to green
    $('.collapse .nav-link').on('click', function () {
        $('.collapse .nav-link').removeClass('active-ul');
        $(this).addClass('active-ul');
    });

    // Prevent the collapse of the dropdown when clicking on submenu items
    $('.collapse').on('click', function (e) {
        e.stopPropagation();
    });
});