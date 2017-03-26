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
        wrapForm: function () {
            var model = this.getModel();
            $("#logoutcontainer").wrap('<form id="frmToSend" name="form" method="post" action="' + model.logoutUrl + '"></form>');
            $("#frmToSend").append("<input type='hidden' name='" + model.antiForgery.name + "' value='" + model.antiForgery.value + "'>");
        }
    };

    return identityServer;
})();

identityServer.wrapForm();