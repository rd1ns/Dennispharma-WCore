 (function($){
  "use strict";

  $(document).ready(function() {
    try {
      $('.ripple').ripples({
        resolution: 512,
            dropRadius: 20,
            perturbance: 0.04,
      });
    }
    catch (e) {
      $('.error').show().text(e);
    }
  });
})(jQuery);