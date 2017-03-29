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