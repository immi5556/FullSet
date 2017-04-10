window.identityServer = (function () {
    "use strict";

    var identityServer = {
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
        pupolateExternalProvider: function () {
            var model = this.getModel();
            model.externalProviders.forEach(function (item, idx) {
                if (((item.text || "").toLowerCase().indexOf("gmail") > -1) ||
                    ((item.text || "").toLowerCase().indexOf("google") > -1)) {
                    $("#externaprovider").append('<li><a href="' + item.href + '" class="gmail"></a></li>');
                }
                if (((item.text || "").toLowerCase().indexOf("facebook") > -1)) {
                    $("#externaprovider").append('<li><a href="' + item.href + '" class="facebook"></a></li>');
                }
                if (((item.text || "").toLowerCase().indexOf("twitter") > -1)) {
                    $("#externaprovider").append('<li style="display:none;"><a href="' + item.href + '" class="twitter"></a></li>');
                }
            });
        },
        wrapForm: function () {
            var model = this.getModel();
            $("#locallogin").wrap('<form id="frmToSend" name="form" method="post" action="' + model.loginUrl + '"></form>');
            $("#frmToSend").append("<input type='hidden' name='" + model.antiForgery.name + "' value='" + model.antiForgery.value + "'>");
        }
    };

    return identityServer;
})();

identityServer.pupolateExternalProvider();
identityServer.wrapForm();
if (identityServer.getModel().errorMessage !== null) {
    $(".invalid").show();
    identityServer.getModel().errorMessage = null;
} else {
    $(".invalid").hide();
}