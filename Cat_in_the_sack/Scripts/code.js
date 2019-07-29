$(document).ready(function() {
    $(".movie").hover( function(){
        $(this).css({
            cursor: 'pointer',
            background: '#2c2c2c',
            transform: 'scale(1.2)',
            transitionDuration: '0.3s'
        })
    }, function(){
        $(this).removeAttr("style");
    }
    );
  });

