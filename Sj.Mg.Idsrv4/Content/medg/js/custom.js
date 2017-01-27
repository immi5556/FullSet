$(document).ready(function(){
    $('body').friendlyLightBox({
        triggerElem : '.click',
        lightCont :'.lightbox',
        shadow:'popupShadow',
        closei:'closeIcon',
        saveData:"#saveData"
    });	
    
    $('.resourceForm').gbMultiSelect({
        permissionList:"#permissionslist",
        selectList:".Listpermission",
        closeModule:".closeIcon",
        closeShadow:".popupShadow",
        scopeData:".scopeData",
        saveData:"#saveData",
        addUser:".addUser"
    });
    var accr = $('.appSection .accordian');
    var tabCont = $('.appSection .tabContent');
    $('.tabContent').hide();
    $(accr[0]).addClass('active');
    $(tabCont[0]).show();
    var flag = true;
    $('.accordian').on('click',function(){
        if(flag){
            flag = false;
            //$('.accordian').removeClass('active');
            $(this).toggleClass('active');
            //$('.tabContent').slideUp();
            $(this).next('.tabContent').slideToggle(500,function(){
                flag = true;
            });
        }
    	
    });
    /*$('.share-cls').on('click',function(){
		var ids = $(this).attr('data-id');
		$(ids).show();
		$('.popupShadow').show();
	});
	$('.closeIcon').on('click',function(){
		$(this).closest('.lightbox').hide();
		$('.popupShadow').hide();
	});
	$('.request-cls').on('click',function(){
		var ids = $(this).attr('data-id');
		$('#requests-id').show();
		$('.popupShadow').show();
	});
	$('.closeIcon').on('click',function(){
		$(this).closest('.lightbox').hide();
		$('.popupShadow').hide();
	});*/
    $('.selectLabel,.selectPermission ul li').on('click',function(e){
        e.stopPropagation()
        $('.selectPermission').addClass('selectPermissionActive');
    });
    $('.scrollSec,.popupShadow,.closeIcon').on('click',function(){
        $('.selectPermission').removeClass('selectPermissionActive');
    })
    //	$('.selectPermission ul li').on('click',function(){
    //		if($(this).hasClass('active')){
    //			$(this).removeClass('active')
    //		}else{
    //			$(this).addClass('active')
    //		}
    //	});
});
var selected;
$(function () {
    var populateselected = function () {
        $("#rsccontt").html("");
        if (details.ScopeUsers[selected]) {
            $(details.ScopeUsers[selected]).each(function (idx, itm) {
                var dd = "<tr><td><a href='javascript:void(0);' class='remove-acc'><img style='width:15px;height:15px;' src ='/Content/medg/images/del_.png' /></a>" + itm + "</td><td><ul><li>Share</li></ul></td></tr>";
                $("#rsccontt").append(dd);
            })
        } else {
            details.ScopeUsers[selected] = [];
        }
    }
    $(document).on("click", "#labdata", function () {
        selected = "patient.MedicationOrder";
        $("#share-id").find("#pop-hdr").html("Medication Data");
        $("#share-id").find("#selectRsrcs").html("patient.MedicationOrder");
        populateselected();
    });
    $(document).on("click", "#obsdata", function () {
        selected = "user.Observation";
        $("#share-id").find("#pop-hdr").html("Observation Data");
        $("#share-id").find("#selectRsrcs").html("user.Observation");
        populateselected();
    });
    $(document).on("click", "#fhirdata", function () {
        selected = "patient/Patient";
        $("#share-id").find("#pop-hdr").html("Fhir Demographics");
        $("#share-id").find("#selectRsrcs").html("patient/Patient");
        populateselected();
    });
    $(document).on("keyup", "#addUser", function (evt) {
        if ($(this).val() == "k" || $(this).val() == "K") {
            $(this).val("Kevin");
            return;
        }
        if ($(this).val() == "s" || $(this).val() == "S") {
            $(this).val("Sven");
            return;
        }
        if ($(this).val() == "j" || $(this).val() == "J") {
            $(this).val("John");
            return;
        }
        if ($(this).val() == "b" || $(this).val() == "B") {
            $(this).val("Bob");
            return;
        }
        if ($(this).val() == "a" || $(this).val() == "A") {
            $(this).val("Andrea");
            return;
        }
    });

    $(document).on("click", ".remove-acc", function () {
        var self = this;
        var tosend = {
            scope: selected,
            access: [],
            touser: $(this).closest("td").text(),
            user: curuser
        };
        $.ajax({
            method: "POST",
            url: "Resource/DeleteDelegation",
            data: tosend
        })
        .done(function (msg) {
            $(self).closest("tr").remove();
        })
        .fail(function (err) {
            alert("Error :" + err);
        });
    });

    $(document).on("click", ".requests-btn-share", function () {
        var self = this;
        var touser = $(this).closest("tr").find("td:eq(0)").text();
        var scope = $(this).closest("tr").find("td:eq(1)").text();
        var tosend = {
            scope: scope,
            access: [],
            touser: touser,
            user: curuser
        }
        $.ajax({
            method: "POST",
            url: "Resource/AllowRequest",
            data: tosend
        })
        .done(function (msg) {
            $(self).closest("tr").remove();
        })
        .fail(function (err) {
            alert("Error :" + err);
        });
    });

    $(document).on("click", ".requests-btn-cancel", function () {
        var self = this;
        var touser = $(this).closest("tr").find("td:eq(0)").text();
        var scope = $(this).closest("tr").find("td:eq(1)").text();
        var tosend = {
            scope: scope,
            access: [],
            touser: touser,
            user: curuser
        }
        $.ajax({
            method: "POST",
            url: "Resource/RemoveRequest",
            data: tosend
        })
        .done(function (msg) {
            $(self).closest("tr").remove();
        })
        .fail(function (err) {
            alert("Error :" + err);
        });
    });
});