(function () {
    'use strict';
    angular
        .module('app')
        .controller('CheckList_MasterController', checkList_MasterController)
    checkList_MasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$filter', '$stateParams', 'Excel', '$timeout']
    function checkList_MasterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $filter, $stateParams, Excel, $timeout) {

        $scope.sortKey = 'ismresgmrE_Id';
        $scope.sortReverse = true;
        $scope.objs = {};

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings!=null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };
        $scope.obj = {};
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        };
        //=====================Adding and removing new row in transcation Grid============      
        $scope.transrows = [{ itrS_Id: 'trans1' }];
        if ($scope.transrows.length == 1) {
            $scope.cnt = 1;
        }
        $scope.addstockrows = function () {
            if ($scope.transrows.length > 1) {
                $scope.cnt = $scope.transrows.length;
            }
            $scope.cnt = $scope.cnt + 1;
            var newItemNo = $scope.cnt;
            $scope.transrows.push({ 'objg': + newItemNo });
        };

        $scope.removestockrows = function (index, data) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);

            if (data.amstB_Id > 0) {
                $scope.Deletestockrows(data);
            }
            if ($scope.transrows.length == 0) {
            }
        };

        //===================Load data=======================
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("Exit_Employee/Get_Checklist", pageid).then(function (promise) {
                $scope.company_list_dd = promise.company_list_dd;
                //          $scope.approver_lisd_dd = promise.approver_lisd_dd;
                $scope.check_list = promise.check_list;
                $scope.presentCountgrid = $scope.check_list.length;

            });
        };

        $scope.oncmpchange = function () {
             $scope.get_department_lisd_dd = [];
            $scope.designation_list_dd = [];
            $scope.objs.hrmD_Id = null;
            $scope.objs.hrmdeS_Id = null;
            var data = {
                "MI_Id": $scope.objs.mI_Id.mI_Id,
            };
            apiService.create("Exit_Employee/company", data).then(function (promise) {
                $scope.get_department_lisd_dd = promise.get_department_lisd_dd;
                //$scope.designation_list_dd = promise.designation_list_dd;

                $scope.loaddeptdesig();
            });

        };

        $scope.get_department = function (objs) {

            var data = {
                "MI_Id": objs.mI_Id.mI_Id
            };
            apiService.create("Exit_Employee/get_company", data).then(function (promise) {

                $scope.get_department_lisd_ddd = promise.get_department_lisd_ddd;
                $scope.designation_list_ddd = promise.designation_list_ddd;

            });
        };

        $scope.get_departmentgrid = function (objs) {
            var data = {
                "MI_Id": objs
            };
            apiService.create("Exit_Employee/get_company", data).then(function (promise) {

                $scope.get_department_lisd_ddd = promise.get_department_lisd_ddd;
                $scope.designation_list_ddd = promise.designation_list_ddd;
                if ($scope.designation_list_ddd != null && $scope.designation_list_ddd.length > 0) {
                    angular.forEach($scope.designation_list_ddd, function (itm) {
                        itm.selected = false;
                    });
                }
            });
        };

        $scope.get_designation = function (obj) {
            $scope.selecteddesgtypes = [];
            $scope.designation_list_dd = [];
            $scope.objs.hrmdeS_Id = null;
            $scope.selecteddesgtypes.push(obj.hrmD_Id);

            if ($scope.selecteddesgtypes.length != 0) {
                $scope.showdesignation = true;
                var data = {
                    emptypes: $scope.selecteddesgtypes,
                }
                apiService.create("LeaveCredit/get_designation", data).
                    then(function (promise) {

                        $scope.designation_list_dd = promise.designation_types;
                    })
            }
            else if ($scope.selecteddesgtypes.length == 0) {
                $scope.Designation_types = $scope.selectedesg;
                $scope.showdesignation = false;
            }
        }



        //================Remove image

        $scope.remove_file = function () {
            $scope.file_detail = "";
            $scope.notice = "";
        };
        $scope.uploadFile = function (input, document) {

            $scope.UploadFile = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" || input.files[0].type == "image/png" || input.files[0].type == "image/jpg" || input.files[0].type === "application/pdf" || input.files[0].type === "application/msword" || input.files[0].type === "application/vnd.ms-excel" &&
                    input.files[0].size <= 2097152) {

                    var reader = new FileReader();

                    reader.onload = function (e) {
                        $('#blahD')
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofile();
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        };
        function Uploadprofile() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadFile.length; i++) {

                formData.append("File", $scope.UploadFile[i]);
                $scope.file_detail = $scope.UploadFile[0].name;
                $scope.photo = $scope.UploadFile[0].name;
            }


            //We can send more data to server using append         
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_Noticefiles", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    $scope.notice = d;
                    if ($scope.notice != null && $scope.notice.length > 0) {
                        var fileurl = $scope.notice[0];
                        $scope.file_detail = $scope.notice[0];
                        var nameArray = fileurl.split('.');
                        $scope.extention = nameArray[nameArray.length - 1];
                        if ($scope.extention === 'doc' || $scope.extention === 'docx' || $scope.extention === 'ppt' || $scope.extention === 'pptx' || $scope.extention === 'xlsx' || $scope.extention === 'xls') {
                            $scope.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + fileurl;
                        }
                    }

                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });


        }


        //=============Save/Edit========================

        $scope.savedata = function () {
            $scope.submitted = true;
            $scope.ApprovalChicklist = [];

            if ($scope.transrows[0].mI_Id != undefined && $scope.transrows[0].hrmD_Id != undefined && $scope.transrows[0].hrmdeS_Id != undefined) {
                angular.forEach($scope.transrows, function (stok) {
                    $scope.ApprovalChicklist.push({
                        MI_Id: stok.mI_Id.mI_Id,
                        HRMD_ID: stok.hrmD_Id.hrmD_Id,
                        HRMDES_ID: stok.hrmdeS_Id.hrmdeS_Id,
                    });
                })
            }

            $scope.notice1 = "";
            if ($scope.notice != undefined && $scope.notice.length > 0) {
                $scope.notice1 = $scope.notice[0];
            }
            else if ($scope.id > 0) {
                $scope.notice1 = $scope.noticeedit;
            }
            var ISMRESGMCL_OptionalFlg = 0;
            if ($scope.ISMRESGMCL_OptionalFlg == true) {
                ISMRESGMCL_OptionalFlg = 1;
            }
            if ($scope.ApprovalChicklist != null && $scope.ApprovalChicklist.length > 0) {
                if ($scope.myForm.$valid) {
                    var data = {
                        "ISMRESGMCL_Id": $scope.id,
                        "MI_Id": $scope.objs.mI_Id.mI_Id,
                        "HRMD_Id": $scope.objs.hrmD_Id.hrmD_Id,
                        "HRMDES_Id": $scope.objs.hrmdeS_Id.hrmdeS_Id,
                        "ISMRESGMCL_Flg": $scope.ChecklistType,
                        "ISMRESGMCL_OptionalFlg": ISMRESGMCL_OptionalFlg,
                        "ISMRESGMCL_Remarks": $scope.ISMRESGMCL_Remarks,
                        "ISMRESGMCL_CheckListName": $scope.ISMRESGMCL_CheckListName,
                        "ISMRESGMCL_Template": $scope.notice1,
                        "ISMRESGMCL_TemplateFileName": $scope.ISMRESGMCL_TemplateFileName,
                        "ApprovalChicklist": $scope.ApprovalChicklist
                    };
                    apiService.create("Exit_Employee/Save_Edit_Checklist", data).then(function (primise) {
                        if (primise.returnval === "Add") {
                            swal('Record save successfully');
                        }
                        else if (primise.returnval === "Update") {
                            swal('Record updated successfully');
                        }
                        else if (primise.returnval === "Exist") {
                            swal('Record already exist');
                        }
                        else {
                            swal('Operation failed');
                        }
                        $state.reload();
                    });
                }
                else {

                    $scope.submitted = true;
                }
            }
            else {
                swal("Select Approval Chicklist Settings");
            }
        };

        $scope.Clearid = function () {
            $state.reload();
        };


        //============================getdetailsReasonmaster================================
        
        //$scope.edit = {};
        
        $scope.edit = function (ss) {
            var pageid = ss.ISMRESGMCL_Id;
            apiService.getURI("Exit_Employee/get_details_checklist", pageid).then(function (promise) {
                $scope.ISMRESGMCL_CheckListName = promise.get_details_check_list[0].ismresgmcL_CheckListName;
                $scope.ChecklistType = promise.get_details_check_list[0].ismresgmcL_Flg;
                $scope.ISMRESGMCL_OptionalFlg = promise.get_details_check_list[0].ismresgmcL_OptionalFlg;
                //

                //$scope.objs.mI_Id = promise.get_details_check_list[0].mI_Id;
                // $scope.newcaste = {};
                if ($scope.company_list_dd != null && $scope.company_list_dd.length > 0) {                    for (var i = 0; i < $scope.company_list_dd.length; i++) {                        if (promise.get_details_check_list[0].mI_Id == $scope.company_list_dd[i].mI_Id) {                            $scope.company_list_dd.Selected = true;                            $scope.objs.mI_Id = $scope.company_list_dd[i];                            $scope.newcaste = $scope.company_list_dd[0].mI_Id;                            // return;                        }                    }                                    }
                $scope.objs.mI_Name = promise.get_details_check_list[0].mI_Name;
                $scope.oncmpchange();
                
         
                $scope.loaddeptdesig = function () {

                    $scope.newcaste1 = {};
                    if ($scope.get_department_lisd_dd != null && $scope.get_department_lisd_dd.length > 0) {                        for (var i = 0; i < $scope.get_department_lisd_dd.length; i++) {                            if (promise.get_details_check_list[0].hrmD_Id == $scope.get_department_lisd_dd[i].hrmD_Id) {                                $scope.get_department_lisd_dd.Selected = true;                                $scope.objs.hrmD_Id = $scope.get_department_lisd_dd[i];                                $scope.newcaste1 = $scope.get_department_lisd_dd[0].hrmD_Id;                                //return;                            }                        }                    }

                    $scope.newcaste12 = {};
                    if ($scope.designation_list_dd != null && $scope.designation_list_dd.length > 0) {                        for (var i = 0; i < $scope.designation_list_dd.length; i++) {                            if (promise.get_details_check_list[0].hrmdeS_Id == $scope.designation_list_dd[i].hrmdeS_Id) {                                $scope.designation_list_dd.Selected = true;                                $scope.objs.hrmdeS_Id = $scope.designation_list_dd[i];                                $scope.newcaste12 = $scope.designation_list_dd[0].hrmdeS_Id;                                //return;                            }                        }                    }
                }
              

                $scope.ISMRESGMCL_Remarks = promise.get_details_check_list[0].ismresgmcL_Remarks;

                $scope.file_detail = promise.get_details_check_list[0].ismresgmcL_Template;
                $scope.ISMRESGMCL_TemplateFileName = promise.get_details_check_list[0].ismresgmcL_TemplateFileName;
                $scope.noticeedit = promise.get_details_check_list[0].ismresgmcL_Template;
                if (promise.get_details_check_list[0].ismresgmcL_Template != null && promise.get_details_check_list[0].ismresgmcL_Template != "") {
                    var nameArray = $scope.file_detail.split('.');
                    $scope.extention = nameArray[nameArray.length - 1];
                    if ($scope.extention === 'doc' || $scope.extention === 'docx' || $scope.extention === 'ppt' || $scope.extention === 'pptx' || $scope.extention === 'xlsx' || $scope.extention === 'xls') {
                        $scope.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + $scope.file_detail;
                    }
                }
                $scope.id = promise.get_details_check_list[0].ismresgmcL_Id;
                $scope.company = {}; $scope.company1 = {}; $scope.company12 = {};
                if (promise.check_list.length > 0) {
                    $scope.transrows = promise.check_list;
                    for (var j = 0; j < $scope.transrows.length; j++) {

                        if ($scope.company_list_dd != null && $scope.company_list_dd.length > 0) {                            for (var i = 0; i < $scope.company_list_dd.length; i++) {                                if ($scope.transrows[j].mI_ID == $scope.company_list_dd[i].mI_Id) {                                    $scope.company_list_dd[i].selected = true;                                    $scope.transrows[j].mI_Id = $scope.company_list_dd[i];                                    $scope.company = $scope.company_list_dd[i].mI_Id;                                }                            }                        }

                        $scope.get_departmentgrid($scope.company);


                        if ($scope.get_department_lisd_ddd != null && $scope.get_department_lisd_ddd.length > 0) {                            for (var k = 0; k < $scope.get_department_lisd_ddd.length; k++) {                                if ($scope.transrows[j].hrmD_ID == $scope.get_department_lisd_ddd[k].hrmD_Id) {                                    $scope.get_department_lisd_ddd[k].selected = true;                                    $scope.transrows[j].hrmD_Id = $scope.get_department_lisd_ddd[k];                                    $scope.company = $scope.get_department_lisd_ddd[k].hrmD_Id;                                }                            }                        }

                        if ($scope.designation_list_ddd != null && $scope.designation_list_ddd.length > 0) {                            for (var l = 0; l < $scope.designation_list_ddd.length; l++) {                                if ($scope.transrows[j].hrmdeS_ID == $scope.designation_list_ddd[l].hrmdeS_Id) {                                    $scope.designation_list_ddd[l].selected = true;                                    $scope.transrows[j].hrmdeS_Id = $scope.designation_list_ddd[l];                                    $scope.company = $scope.designation_list_ddd[l].hrmdeS_Id;                                }                            }                        }

                    }
                }
                
                

            });
        };

        $scope.ViewApproverDepartDesign = function (obj) {
            var data = {
                "ISMRESGMCL_Id": obj.ISMRESGMCL_Id,
            };

            apiService.create("Exit_Employee/get_ApproverDepartDesign_checklist", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.get_details_Approver_list !== null && promise.get_details_Approver_list.length > 0) {
                        $scope.get_details_Approver_list = promise.get_details_Approver_list;
                        $('#modalviewfiles').modal('show');
                    } else {
                        swal("No Records Found");
                        $('#modalviewfiles').modal('hide');
                    }
                }
            });
        };

        $scope.deactive = function (flr, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (flr.ISMRESGMCL_ActiveFlag === false) {
                mgs = "Deactivate";
            } else {
                mgs = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Master Check List?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Exit_Employee/active_deactive_checklist", flr).
                            then(function (promise) {

                                if (promise.returnval === "false") {
                                    swal('Master Check list Deactivated Successfully');
                                }

                                else if (promise.returnval === "true") {
                                    swal('Master Check list Activated Successfully');
                                }
                                else {
                                    swal("Cancelled");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Cancelled");
                        $state.reload();
                    }
                });
        };
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.showmodaldetails = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];

            if (extention == "jpg" || extention == "jpeg") {
                $('#preview').attr('src', data);
            }
            else if (extention == "doc" || extention == "docx") {
                $('#preview').removeAttr('src');
            }
            else if (extention == "pdf") {
                var imagedownload = data;
                $scope.content = "";
                var fileURL = "";
                var file = "";
                $http.get(imagedownload, { responseType: 'arraybuffer' })
                    .success(function (response) {
                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        var pdfId = document.getElementById("pdfIdzz");
                        pdfId.removeChild(pdfId.childNodes[0]);
                        var embed = document.createElement('embed');
                        embed.setAttribute('src', fileURL);
                        embed.setAttribute('type', 'application/pdf');
                        embed.setAttribute('width', '100%');
                        embed.setAttribute('height', '1000');
                        pdfId.appendChild(embed);
                        $('#showpdf').modal('show');

                        file = new Blob([(response)], { type: 'application/pdf' });
                        fileURL = URL.createObjectURL(file);
                        $scope.content = $sce.trustAsResourceUrl(fileURL);
                        $('#showpdf').modal('show');
                    });
            }
        };
    }

})();
