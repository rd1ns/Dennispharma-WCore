;(function($){
"use strict";
    
    /*----------------------------------------------------*/
    /*  offcanvasActivator
    /*----------------------------------------------------*/   
    function offcanvasActivator(){
        if ( $('#offcanvas_menu').length ){
            $('#offcanvas_menu').on('click', function(){
                $('.offcanvas_menu,.offcanvas_closer').toggleClass('open')
            });
            $('.offcanvas_closer,.close-offcanvas').on('click',function(){
                $('.offcanvas_menu,.offcanvas_closer').removeClass('open')
            })
        }
    }
    offcanvasActivator();
    
    $('a[href="#"]').on('click', function(e){
        e.preventDefault();
    })
    
    /*----------------------------------------------------*/
    /*  Images grid Filter
    /*----------------------------------------------------*/   
    function ImagesgridFilter(){
        if( $('.image-grid, .single_blog_gallery').length ){
            $('.image-grid, .single_blog_gallery').imagesLoaded(function(){
                $('.image-grid, .single_blog_gallery').isotope({
                    itemSelector: '.grid, .item_blog',
                    layoutMode: 'masonry',
                    masonry: {
                        columnWidth: 1,
                    }
                })
            });
        }
    }
    ImagesgridFilter();

    
    /*----------------------------------------------------*/
    /*  counterUp
    /*----------------------------------------------------*/   
    function counterUp(){
        if( $('.counter').length ){
            $('.counter').counterUp({
                delay: 10,
                time: 1000
            });
        }
    }
    
    counterUp();
    
    /*----------------------------------------------------*/
    /*  Go To
    /*----------------------------------------------------*/        
    $('a[href^="#"]#mouse, a[href^="#"].keep-scroll').on('click', function(event) {

        var target = $( $(this).attr('href') );

        if( target.length ) {
            event.preventDefault();
            $('html, body').animate({
                scrollTop: target.offset().top
            }, 1000)
        }

    });
    
    
    
/// stick menu function
    
    var nav_offset_top = $('.header').offset().top;
    
    if ( nav_offset_top == 0 ){
        nav_offset_top = 1
    }
    
    $('.header').affix({
        offset: {
            top: nav_offset_top,
        }
    });
    
    /*----------------------------------------------------*/
    /*  Preloader
    /*----------------------------------------------------*/
    $(window).on('load', function(){
        $('.preloader').addClass('complete');
        setTimeout(
            function(){
                $('.preloader').fadeOut('slow')    
            },2100
        );
    });
    
    
})(jQuery)