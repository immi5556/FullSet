//(function () {

//    $(".tabSection").myTabs({

//    });

//    $(window).resize(secHeight())
//    secHeight();

//    function secHeight() {
//        var Wh = $(window).height() - 480;
//        if (Wh > 300) {
//            $('.contentRight').height(Wh);
//            var Hh = Wh / 2;
//            $('.viewSectionList').height(Hh - 50);
//            $('.viewShareSectionList').height(Hh - 50);
//            $('.requestingSectionList').height(Wh - 80);
//            //console.log(Hh)
//        } else {
//            $('.contentRight').css({
//                "height": 300 + 'px'
//            });
//            $('.viewSection').css({
//                "height": 150 + 'px'
//            });
//            $('.viewShareSection').css({
//                "height": 150 + 'px'
//            });
//            $('.viewShareSectionList').css({
//                "height": 110 + 'px'
//            });
//            $('.requestingSectionList').css({
//                "height": 110 + 'px'
//            });
//            $('.requestingSectionList').css({
//                "height": 220 + 'px'
//            });
//        }
//    };


//})();

var identityServer = function () {
    return {
        getModel: function () {
            var modelJson = document.getElementById("modelJson");
            var encodedJson = '';
            if (typeof (modelJson.textContent) !== undefined) {
                encodedJson = modelJson.textContent;
            } else {
                encodedJson = modelJson.innerHTML;
            }
            var json = Encoder.htmlDecode(encodedJson);
            var model = JSON.parse(json);
            return model;
        },
        wrapForm: function () {
            var model = this.getModel();
            $(".logout").attr("href", model.logoutUrl);
        }
    }
}();
window.location.href = "https://localhost:44383/Home/Secure";
//window.location.href = "https://oidc.medgrotto.com:9001/Home/Secure";
identityServer.wrapForm();