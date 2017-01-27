(function($){
    $.fn.gbMultiSelect = function(opt){
        var defaults = {
            permissionList:"#permissionslist",
            selectList:".Listpermission",
            scopeData:".scopeData",
            saveData:"#saveData",
            addUser:".addUser"
        },
        set = $.extend({},defaults,opt);
        
        return this.each(function(){
            var $this = $(this),
                ULlist = $this.find(set.permissionList),
                list = ULlist.find('li'),
                checkedItems = [],
                filterData = [],
                selectPermissions = $this.find(set.selectList),
                selectedpermission,
                tableScopeData = '';
            checkedItems.push("Share");
                
                list.each(function(ind,ele){
                    $(this).find('input').on('change',function(){
                       var thisVal = $(this).attr('data-text');
                        selectPermissions.html('');
                        if(this.checked){
                            if(checkedItems.indexOf(thisVal) ==-1){
                                checkedItems.push(thisVal);
                            }
                        }else{
                            for(var i=0; i < checkedItems.length; i++){
                                if(checkedItems[i] === thisVal){
                                    checkedItems.splice(i,1);
                                }
                            }
                        };
                       
                        dropList(checkedItems);
                        
                        
                        
                        console.log(selectedpermission)
                        
                        if(checkedItems ==''){
                            selectPermissions.html('');
                        }
                    });
                    function dropList(checkedItems){
                        
                        var listData = $('<ul></ul>');
                        selectPermissions.html('');
                        var x=0;
                        for(x; x < checkedItems.length; x++){
                            var list = $('<li>'+checkedItems[x]+'</li>');
                            listData.append(list);
                            console.log(listData)
                        }
                        console.log(checkedItems)
                        selectPermissions.append(listData);
                        selectedpermission = listData.find('li');
                        
                        for(var y=0; y < selectedpermission.length; y++){
                            $(selectedpermission[y]).on('click',function(){
                                var thisText = $(this).text();
                                var perList = ULlist.find('li');
                                for(var i=0; i < checkedItems.length; i++){
                                    if(checkedItems[i] === thisText){
                                        checkedItems.splice(i,1);
                                    }
                                }
                                for(var i=0; i < perList.length; i++){
                                    if($(perList[i]).text() == thisText){
                                        $(perList[i]).find('input').attr('checked',false);
                                    }
                                }
                                if(checkedItems.length > 0){
                                    dropList(checkedItems);
                                }else {
                                  selectPermissions.html('');  
                                }
                                
                                
                            });
                        }
                    }
                });
            
            //Savedata

            $(set.saveData).on('click',function(){
                if ($(set.addUser).val() !== '' && $(set.addUser).val() !== ' ' && checkedItems.length) {
                    var userName = $(set.addUser).val();
                    tableScopeData ='<tr>';
                    tableScopeData += '<td><a href="javascript:void(0);" class="remove-acc"><img style="width:15px;height:15px;" src ="/Content/medg/images/del_.png" /></a>' + userName + '</td>';
                    if(checkedItems.length > 0){
                        tableScopeData+='<td>';
                        tableScopeData+='<ul>';
                        for(var i=0; i < checkedItems.length; i++){
                            tableScopeData+='<li>'+checkedItems[i]+'</li>';
                        }
                        tableScopeData+='</ul>';
                        tableScopeData+='</td>';
                    }
                     tableScopeData +='</tr>';
                     var tosend = {
                         scope: selected,
                         access: checkedItems,
                         touser: userName,
                         user: curuser
                     }
                     $.ajax({
                         method: "POST",
                         url: "Resource/SaveDelegation",
                         data: tosend
                     })
                       .done(function (msg) {
                           //alert("Data Saved: " + msg);
                           if ($.inArray(tosend.touser, details.ScopeUsers[selected]) > -1) {
                               $(set.scopeData).find("tr td:contains('" + tosend.touser + "')").closest("tr").remove();
                           } else {
                               details.ScopeUsers[selected].push(tosend.touser);
                           }
                           $(set.scopeData).append(tableScopeData);
                           listGenarator();
                       })
                     .fail(function (err) {
                         alert("Error :" + err);
                     });
                     $(set.addUser).val('');
                     checkedItems =[ "Share" ];
                     selectPermissions.html('');
                     var perList = ULlist.find('li');
                     for(var i=0; i < perList.length; i++){
                        $(perList[i]).find('input').attr('checked',false);
                     }
                }
                else {
                    alert('Fill Name')
                }
            });
            listGenarator();
            function listGenarator(){
                var tableListItems = $(set.scopeData).find('ul').children();
                for(var x =0; x < tableListItems.length; x++){
                    $(tableListItems[x]).on('click',function(){
                        $(this).remove();
                    })
                }
            }
        });
    }
})(jQuery)