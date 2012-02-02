(function($) {
  $.fn.textboxhelp = function(options){
    var defaults = {help: 'Help me!', focuscls:'focus'};
    // merge defaults with coming parameters
    var opts = $.extend(defaults, options);
    $(this).val(opts.help);
    $(this).focus(function(){
      // keep the text for check
      var t = $(this).val();
      // when user focus to textbox, we should clean inside it
      // but first we should check exist text
      if(t.length > 0 && t == opts.help)
        $(this).addClass(opts.focuscls).val('');
    }).blur(function() { 
      // keep the text for later check
      var t = $(this).val();
      // if textbox is empty, we will add help text inside it again
      if('' == t)
         $(this).removeClass(opts.focuscls).val(opts.help);
    });
  }
})(jQuery); 
