(function ($) {
    "use strict";



    //Burdan Basla Blet


    // Tab-menu
    $(document).on('click', '.category', function (e) {
        e.preventDefault();
        $(this).addClass('active').siblings().removeClass('active');
        const category = $(this).attr('category-id');
        const products = $('.fruite-item');

        products.each(function () {
            if (category === $(this).attr('category-id')) {
                $(this).parent().fadeIn();
            }
            else {
                $(this).parent().hide();
            }
        })

        if (category == 'All') {
            products.parent().fadeIn();
        }
    });

    //Sorting
    $('.sorting').on('change', function () {
        const value = $(this).val().trim();

        $(".paginate").css("display", "none");

        $(".products .product-item").slice(0).remove();

        $.ajax({
            type: "Get",
            url: `Shop/Sorting?sort=${value}`,
            success: function (res) {
                $('.products').append(res);
            },
        });
    });

    // Category filter
    $('.category-filter').on('click', function () {
        const categoryId = $(this).attr('category-id');

        $(".paginate").css("display", "none");

        $(".products .product-item").slice(0).remove();

        $.ajax({
            type: "Get",
            url: `Shop/CategoryFilter?id=${categoryId}`,
            success: function (res) {
                $('.products').append(res);
            },
        });
    });

    //Price filter
    $('.form-range').on('change', function () {
        const value = $(this).val().trim();

        $(".paginate").css("display", "none");

        $(".products .product-item").slice(0).remove();

        $.ajax({
            type: "Get",
            url: `Shop/PriceFilter?price=${value}`,
            success: function (res) {
                $('.products').append(res);
            },
        });
    });


    // Add
    $(document).on("click", ".add-basket", function () {
        const productId = $(this).attr('product-id');

        $.ajax({
            type: "Post",
            url: `Basket/Add?productId=${productId}`,
            success: function (res) {
                Swal.fire({
                    icon: "success",
                    title: "Added to cart!",
                    showConfirmButton: false,
                    timer: 1500
                });

                const count = $(".basket-count");
                count.text(parseInt(count.text()) + 1);
            }
        });
    });

    // Increase
    $(document).on("click", ".btn-plus", function () {
        const id = $(this).attr('id');
        const btn = $(this);

        $.ajax({
            type: "Post",
            url: `Basket/Increase?id=${id}`,
            success: function (res) {

                btn.closest(".basket-item").find(".total").text(res.totalPrice + ' $');

                const count = $(".basket-count");
                count.text(parseInt(count.text()) + 1);

                $(".total-price").text('$' + res.total);
            }
        });
    });

    // Decrease
    $(document).on("click", ".btn-minus", function () {
        const id = $(this).attr('id');
        const btn = $(this);

        $.ajax({
            type: "Post",
            url: `Basket/Decrease?id=${id}`,
            success: function (res) {

                const basketItem = btn.closest(".basket-item");

                if (basketItem.find(".quantity input").val() == 0) {
                    basketItem.remove();
                } else {
                    basketItem.find(".total").text(res.totalPrice + ' $');
                }

                const count = $(".basket-count");
                count.text(parseInt(count.text()) - 1);

                $(".total-price").text('$' + res.total);
            }
        });
    });

    // Delete
    $(document).on("click", ".delete-basket", function () {
        const id = $(this).attr('id');
        const btn = $(this);

        $.ajax({
            type: "Post",
            url: `Basket/Delete?id=${id}`,
            success: function (res) {
                btn.closest(".basket-item").remove();

                const count = $(".basket-count");
                count.text(parseInt(count.text()) - 1);

                $(".total-price").text("$" + res);
            }
        });
    });

    // Spinner
    var spinner = function () {
        setTimeout(function () {
            if ($('#spinner').length > 0) {
                $('#spinner').removeClass('show');
            }
        }, 1);
    };
    spinner(0);


    // Fixed Navbar
    $(window).scroll(function () {
        if ($(window).width() < 992) {
            if ($(this).scrollTop() > 55) {
                $('.fixed-top').addClass('shadow');
            } else {
                $('.fixed-top').removeClass('shadow');
            }
        } else {
            if ($(this).scrollTop() > 55) {
                $('.fixed-top').addClass('shadow').css('top', -55);
            } else {
                $('.fixed-top').removeClass('shadow').css('top', 0);
            }
        } 
    });
    
    
   // Back to top button
   $(window).scroll(function () {
    if ($(this).scrollTop() > 300) {
        $('.back-to-top').fadeIn('slow');
    } else {
        $('.back-to-top').fadeOut('slow');
    }
    });
    $('.back-to-top').click(function () {
        $('html, body').animate({scrollTop: 0}, 1500, 'easeInOutExpo');
        return false;
    });


    // Testimonial carousel
    $(".testimonial-carousel").owlCarousel({
        autoplay: true,
        smartSpeed: 2000,
        center: false,
        dots: true,
        loop: true,
        margin: 25,
        nav : true,
        navText : [
            '<i class="bi bi-arrow-left"></i>',
            '<i class="bi bi-arrow-right"></i>'
        ],
        responsiveClass: true,
        responsive: {
            0:{
                items:1
            },
            576:{
                items:1
            },
            768:{
                items:1
            },
            992:{
                items:2
            },
            1200:{
                items:2
            }
        }
    });


    // vegetable carousel
    $(".vegetable-carousel").owlCarousel({
        autoplay: true,
        smartSpeed: 1500,
        center: false,
        dots: true,
        loop: true,
        margin: 25,
        nav : true,
        navText : [
            '<i class="bi bi-arrow-left"></i>',
            '<i class="bi bi-arrow-right"></i>'
        ],
        responsiveClass: true,
        responsive: {
            0:{
                items:1
            },
            576:{
                items:1
            },
            768:{
                items:2
            },
            992:{
                items:3
            },
            1200:{
                items:4
            }
        }
    });


    // Modal Video
    $(document).ready(function () {
        var $videoSrc;
        $('.btn-play').click(function () {
            $videoSrc = $(this).data("src");
        });
        console.log($videoSrc);

        $('#videoModal').on('shown.bs.modal', function (e) {
            $("#video").attr('src', $videoSrc + "?autoplay=1&amp;modestbranding=1&amp;showinfo=0");
        })

        $('#videoModal').on('hide.bs.modal', function (e) {
            $("#video").attr('src', $videoSrc);
        })
    });



    // Product Quantity
    $('.quantity button').on('click', function () {
        var button = $(this);
        var oldValue = button.parent().parent().find('input').val();
        if (button.hasClass('btn-plus')) {
            var newVal = parseFloat(oldValue) + 1;
        } else {
            if (oldValue > 0) {
                var newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 0;
            }
        }
        button.parent().parent().find('input').val(newVal);
    });

})(jQuery);

