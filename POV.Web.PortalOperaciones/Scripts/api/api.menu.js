var MenuApi = function () { };
//.slideDown('fast')
    MenuApi.prototype.BuildMenu
        = function () {
            $("ul.subnav").parent().append("<span></span>");
            
            $("a.menunavtop").hover(function () {
                $(this).addClass("subhover");
                $(this).parent().find("ul.subnav").show();
                $(this).parent().hover(function () {
                }, function () {
                    $(this).parent().find("ul.subnav").slideUp('fast');
                });
            }, function () {
                $(this).removeClass("subhover");
            });

            $("a.submenuav").hover(function () {
                $(this).addClass("subhover");
                $(this).parent().find("ul.subsubnav").show();
                $(this).parent().hover(function () {
                }, function () {
                    $(this).parent().find("ul.subsubnav").slideUp('fast');
                });
            }, function () {
                $(this).removeClass("subhover");
            });

            $("ul.topnav li span").click(function() {
                $(this).parent().find("ul.subnav").show();
                $(this).parent().hover(function() {
                }, function() {
                    $(this).parent().find("ul.subnav").slideUp('fast');
                });
            }).hover(function() {
                $(this).addClass("subhover");
                $(this).parent().find("ul.subnav").show();
                $(this).parent().hover(function () {
                }, function () {
                    $(this).parent().find("ul.subnav").slideUp('fast');
                });
            }, function() {
                $(this).removeClass("subhover");
            });
        };

              

