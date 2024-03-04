/* jQuery Notice Plugin
 * 
 * This plug-in allows you to show a little notice box.
 * 
 * @author: Vincent Composieux
 * @homepage: http://vincent.composieux.fr
 * @date 20/06/2010
 */

(function($) {
	$.notice = {
		show: function(message) {
			/** Configuration */
            
		    var micuenta = $("li.mPosicion");
		    var posicion = micuenta.position();

		    var top = posicion.top + 8;//20;
		    var left = posicion.left;//-25;
		    var width = $(window).width();		    
		    var fadeoutDuration = 10000;

		    width = 100 - ((335 * 100) / width);

		    if (width < 10)
		        width = width *2;

		    if (width < 4)
		        width = width *2 ;

		    if (width < 0)
		        width = width + (width * (-1.1));

		    /** Launch the notification */
			$('html, body').animate({ /*scrollTop: 0*/ });
			$('<div></div>').attr('class', 'notice').css('left', (width)+'%').css('top', (top)+'px').appendTo('body').text(message);
			
			/** Switch off the notification */
			setTimeout(function () {
			    $('div.notice').animate({
			        opacity: 0, top: '-20px'
			    }, fadeoutDuration);
			}, 2000);
			setTimeout(function () {
			    $('div.notice').remove();
			}, 4000);
		}
	}
	
	jQnotice = function(message) { $.notice.show(message); };
})(jQuery);