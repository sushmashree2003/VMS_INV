(function () {
    'use strict';
    angular
        .module('app')
        .controller('AssetTagController', AssetTagController);
    AssetTagController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q', '$window'];
    function AssetTagController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $window) {

        $scope.obj = {};
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //========================================== exp date
        $scope.expdate = function (qq, ww) {
            $scope.newdate = "";
            if (qq.invaaT_PurchaseDate && qq.invaaT_WarantyPeriod) {
                var newdate = new Date(qq.invaaT_PurchaseDate);
                if (!isNaN(newdate.getTime())) {
                    newdate.setDate(newdate.getDate() + parseInt(qq.invaaT_WarantyPeriod));
                    qq.invaaT_WarantyExpiryDate = new Date(newdate);
                }
            }
        };
        //=======================================PAGE LOAD
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage = 100;
            $scope.itemsPerPage1 = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("AssetTag/getloaddata", pageid).
                then(function (promise) {
                    $scope.edits_tr = false;
                    $scope.get_store = promise.get_store;
                    $scope.get_Assetstag = promise.get_Assetstag;
                    $scope.presentCountgrid1 = $scope.get_Assetstag.length;
                });
        };
        //========================================= STORE CHANGE
        $scope.onstorechange = function () {
            var data = {
                "INVMST_Id": $scope.invmsT_Id
            };
            apiService.create("AssetTag/getdata", data).
                then(function (promise) {
                    $scope.get_items = promise.get_items;
                });
        };
        //========================================= ITEM CHANGE
        $scope.onItemchange = function () {
            $scope.get_tagdata = [];
            var ActualQty = 0; var INVMI_Id = 0; var INVSTO_PurchaseRate = 0;
            angular.forEach($scope.get_items, function (at) {
                if ($scope.obj.INVMI_Id.INVSTO_Id == at.INVSTO_Id) {
                    ActualQty = at.ActualQty;
                    INVMI_Id = at.INVMI_Id;
                    INVSTO_PurchaseRate = at.INVSTO_PurchaseRate;
                }
            });
            var data = {
                "INVMST_Id": $scope.invmsT_Id,
                "INVMI_Id": INVMI_Id,
                "PurchaseRate": INVSTO_PurchaseRate,
                "INVSTO_Id": $scope.obj.INVMI_Id.INVSTO_Id
            };
            apiService.create("AssetTag/getdata", data).
                then(function (promise) {
                    $scope.edits_tr = false;
                    if (promise.get_tagdata != null && promise.get_tagdata.length > 0) {
                        angular.forEach(promise.get_tagdata, function (at) {
                            if (at.ActualQty == ActualQty) {
                                at.FilePath = "";
                                at.FileName = "";
                                //sushma
                                if (at.invaaT_PurchaseDate != null && at.invaaT_PurchaseDate != undefined && at.invaaT_PurchaseDate != "") {
                                    at.invaaT_PurchaseDate = new Date(at.invaaT_PurchaseDate);
                                }
                                if (at.invaaT_ManufacturedDate != null && at.invaaT_ManufacturedDate != undefined && at.invaaT_ManufacturedDate != "") {
                                    at.invaaT_ManufacturedDate = new Date(at.invaaT_ManufacturedDate);
                                }
                                if (at.invaaT_WarantyExpiryDate != null && at.invaaT_WarantyExpiryDate != undefined && at.invaaT_WarantyExpiryDate != "") {
                                    at.invaaT_WarantyExpiryDate = new Date(at.invaaT_WarantyExpiryDate);
                                }
                                if (at.invaaT_AMCExpiryDate != null && at.invaaT_AMCExpiryDate != undefined && at.invaaT_AMCExpiryDate != "") {
                                    at.invaaT_AMCExpiryDate = new Date(at.invaaT_AMCExpiryDate);
                                }       
                                $scope.get_tagdata.push(at);
                            }
                        });
                    }


                    $scope.presentCountgrid = $scope.get_tagdata.length;

                });
        };

        //sushma
        $scope.SelectedFileForUploadzd = [];
        $scope.materaldocuupload = [{ id: 'materal' }];

        $scope.selectFileforUploadzd = function (input, index) {
            $scope.SelectedFileForUploadzd = input.files;
            $scope.vbabrsF_FileName = input.files[0].name;
            if (input.files && input.files[0]) {
                let file = input.files[0];
                let fileType = file.type;
                let fileSize = file.size;
                //if (fileType !== "application/pdf") {
                //    swal("Only PDF files are allowed!");
                //    input.value = "";
                //    return;
                //}

                UploaddianmateralPhoto(file, index);
            }
        };
        function UploaddianmateralPhoto(file, index) {
            //  console.log("check Book upload  Upload  :" + data);
            var formData = new FormData();
            formData.append("File", $scope.SelectedFileForUploadzd[0]);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadtrnsportdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    console.log("URL", d);
                    $scope.get_tagdata[index].FilePath = d;
                    //data.vbabrsF_FilePath = d;
                    //data.vbabrsF_FileName = $scope.vbabrsF_FileName;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });



        };

        //======================================== Check Duplicate        
        $scope.checkduplicate = function (tagdata) {
            //sushma
            if ($scope.edits == true) {

            }
            else {
                $scope.tag_list = [];
                angular.forEach(tagdata, function (tg) {
                    if (tg.xyz) {
                        if ($scope.tag_list.length === 0) {
                            $scope.tag_list.push(tg);
                        }
                        else if ($scope.tag_list.length > 0) {
                            angular.forEach($scope.tag_list, function (tl) {
                                if (tg.INVMI_Id === tl.INVMI_Id) {
                                    if (tg.invaaT_AssetId === tl.invaaT_AssetId) {
                                        swal("Tag already exist....!!");
                                        tg.invaaT_AssetId = "";
                                    }
                                }
                            });
                        }
                        angular.forEach($scope.get_Assetstag, function (at) {
                            if (at.invmI_Id === tg.INVMI_Id) {
                                if (at.invaaT_AssetId === tg.invaaT_AssetId) {
                                    swal("Tag already assigned....!!");
                                    tg.invaaT_AssetId = "";
                                }
                            }
                        });
                    }
                });
            }

        };
        //======================================= Grid Check box Selection
        $scope.toggleAll = function () {
            angular.forEach($scope.get_tagdata, function (subj) {
                subj.xyz = $scope.all;
            });
        };
        $scope.optionToggled = function () {
            $scope.all = $scope.get_tagdata.every(function (itm) { return itm.xyz; });
        };
        //=====================================  SAVE DATA
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.tagckdArray = [];
                if ($scope.edits_tr == true) {
                    angular.forEach($scope.get_tagdata, function (tag) {
                        if (tag.xyz) {
                            $scope.tagckdArray.push({
                                invmsT_Id: $scope.invmsT_Id, invmI_Id: tag.INVMI_Id, invaaT_AssetId: tag.invaaT_AssetId, invaaT_AssetDescription: tag.invaaT_AssetDescription, invaaT_ModelNo: tag.invaaT_ModelNo,
                                invaaT_SerialNo: tag.invaaT_SerialNo, invaaT_SKU: tag.invaaT_SKU, invaaT_ManufacturedDate: tag.invaaT_ManufacturedDate, invaaT_PurchaseDate: tag.invaaT_PurchaseDate,
                                invaaT_WarantyPeriod: tag.invaaT_WarantyPeriod, invaaT_WarantyExpiryDate: tag.invaaT_WarantyExpiryDate, invaaT_UnderAMCFlg: tag.invaaT_UnderAMCFlg,
                                invaaT_AMCExpiryDate: tag.invaaT_AMCExpiryDate, staffcheck: tag.FilePath
                                
                            });
                        }
                    });
                }
                else {
                    angular.forEach($scope.get_tagdata, function (tag) {
                        if (tag.xyz) {
                            $scope.tagckdArray.push({
                                invmsT_Id: $scope.invmsT_Id, invmI_Id: tag.INVMI_Id, invaaT_AssetId: tag.invaaT_AssetId, invaaT_AssetDescription: tag.invaaT_AssetDescription, invaaT_ModelNo: tag.invaaT_ModelNo,
                                invaaT_SerialNo: tag.invaaT_SerialNo, invaaT_SKU: tag.invaaT_SKU, invaaT_ManufacturedDate: tag.invaaT_ManufacturedDate, invaaT_PurchaseDate: tag.invaaT_PurchaseDate,
                                invaaT_WarantyPeriod: tag.invaaT_WarantyPeriod, invaaT_WarantyExpiryDate: tag.invaaT_WarantyExpiryDate, invaaT_UnderAMCFlg: tag.invaaT_UnderAMCFlg,
                                invaaT_AMCExpiryDate: tag.invaaT_AMCExpiryDate, staffcheck: tag.FilePath
                                
                            });
                        }
                    });
                }

                console.log($scope.tagckdArray);
                //return;
                if ($scope.tagckdArray.length > 0) {
                    var data = {
                        "INVAAT_Id": $scope.invaaT_Id,
                        "tagckdArray": $scope.tagckdArray
                    };
                }
                else {
                    swal("Select Atleast One checkbox....!!");
                }
                apiService.create("AssetTag/savedata", data).then(function (promise) {
                    if (promise.returnval === true) {
                        if (promise.invaaT_Id === 0 || promise.invaaT_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invaaT_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invaaT_Id === 0 || promise.invaaT_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invaaT_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };
        $scope.deactive = function (item) {
            $scope.INVAAT_Id = item.invaaT_Id;
            var dystring = "";
            if (item.invaaT_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invaaT_ActiveFlg === false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("AssetTag/deactive", item).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        };
        $scope.edit = function (item) {
            var data = {
                "INVMST_Id": item.invmsT_Id,
                "INVAAT_Id": item.invaaT_Id
            }

            apiService.create("AssetTag/get_edit_data", data).then(function (promise) {
                $scope.edits = true;
                $scope.edits_tr = true;
                $scope.get_tagdata = [];
                $scope.taglist = [];
                $scope.invmS_StoreName = promise.get_edit_data[0].invmS_StoreName;
                $scope.invmI_ItemName = promise.get_edit_data[0].invmI_ItemName;
                $scope.invmsT_Id = promise.get_edit_data[0].invmsT_Id;
                $scope.invmI_Id = promise.get_edit_data[0].invmI_Id;
                $scope.invaaT_Id = promise.get_edit_data[0].invaaT_Id;




                // $scope.get_store = promise.get_edit_store;
                //$scope.get_items = promise.get_edit_item;

                $scope.taglist.push(item);
                console.log("$scope.taglist", $scope.taglist);
                $scope.presentCountgrid = $scope.taglist.length;
                angular.forEach($scope.taglist, function (li) {
                    debugger;
                    var perdate = null;
                    var expdate = null;
                    var amcdate = null;
                    var mandate = null;
                    if (li.invaaT_WarantyExpiryDate != null && li.invaaT_WarantyExpiryDate != undefined) {
                        expdate = new Date(li.invaaT_WarantyExpiryDate)
                    }
                    if (li.invaaT_AMCExpiryDate != null && li.invaaT_AMCExpiryDate != undefined) {
                        amcdate = new Date(li.invaaT_AMCExpiryDate)
                    }
                    if (li.invaaT_PurchaseDate != null && li.invaaT_PurchaseDate != undefined) {
                        perdate = new Date(li.invaaT_PurchaseDate)
                    }
                    if (li.invaaT_ManufacturedDate != null && li.invaaT_ManufacturedDate != undefined) {
                        mandate = new Date(li.invaaT_ManufacturedDate)
                    }
                    if (li.invaaT_UnerAMCFlg == undefined || li.invaaT_UnerAMCFlg == null) {
                        li.invaaT_UnerAMCFlg = false;
                    }
                    $scope.get_tagdata.push({
                        INVMI_Id: li.invmI_Id, INVMI_ItemName: li.invmI_ItemName, invaaT_AssetId: li.invaaT_AssetId,
                        invaaT_AssetDescription: li.invaaT_AssetDescription, invaaT_ModelNo: li.invaaT_ModelNo, invaaT_SerialNo: li.invaaT_SerialNo,
                        invaaT_SKU: li.invaaT_SKU, invaaT_ManufacturedDate: mandate, invaaT_PurchaseDate: perdate,
                        invaaT_WarantyPeriod: li.invaaT_WarantyPeriod, invaaT_WarantyExpiryDate: expdate,
                        invaaT_UnderAMCFlg: li.invaaT_UnerAMCFlg, invaaT_AMCExpiryDate: amcdate, xyz: true,
                        FilePath: li.staffcheck


                    });
                });
            });
        };
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
        $scope.searchValue1 = '';


        $scope.previewimg_new = function (img) {
            $scope.vbabrsF_FilePath = img;
            var img = $scope.vbabrsF_FilePath;
            if (img != null) {
                var imagarr = img.split('.');
                var lastelement = imagarr[imagarr.length - 1];
                if (lastelement == 'jpg' || lastelement == 'jpeg' || lastelement == 'png') {
                    $('#preview').attr('src', $scope.vbabrsF_FilePath);
                    $('#myimagePreview').modal('show');
                }
                else if (lastelement == 'doc' || lastelement == 'docx' || lastelement == 'xls' || lastelement == 'xlsx' || lastelement == 'ppt' || lastelement == 'pptx') {
                    $window.open($scope.vbabrsF_FilePath);
                }
                else if (lastelement == 'pdf') {
                    $('#showpdf').modal('hide');
                    var imagedownload1 = "";
                    imagedownload1 = $scope.vbabrsF_FilePath;
                    $http.get(imagedownload1, { responseType: 'arraybuffer' })
                        .success(function (response) {
                            var fileURL = "";
                            var file = "";
                            var embed = "";
                            var pdfId = "";
                            file = new Blob([(response)], { type: 'application/pdf' });
                            fileURL = URL.createObjectURL(file);
                            pdfId = document.getElementById("pdfIdzz");
                            pdfId.removeChild(pdfId.childNodes[0]);
                            embed = document.createElement('embed');
                            embed.setAttribute('src', fileURL);
                            embed.setAttribute('type', 'application/pdf');
                            embed.setAttribute('width', '100%');
                            embed.setAttribute('height', '1000');
                            pdfId.appendChild(embed);
                            $('#showpdf').modal('show');
                        });
                }
            }
            else {
                $window.open($scope.vbabrsF_FilePath)
            }
        };

    }
})();