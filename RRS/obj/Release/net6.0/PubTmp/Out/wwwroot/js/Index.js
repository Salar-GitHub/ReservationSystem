$(window).scroll(function () {

    var s = $(window).scrollTop(),

        opacityVal = (s / 150.0);
        antiOpacityVal = (100 - s)

    $('#nav-index-2').css('opacity', opacityVal);

    $('.background-blur').css('opacity', opacityVal);
    $(".index-flex-container").css({ "left": 0 - s * 3 + "px" });
    $(".iphone-flex").css({ "right": -450 + s * 1.1 + "px" });
    $('.down-icon').css('opacity', antiOpacityVal);
});