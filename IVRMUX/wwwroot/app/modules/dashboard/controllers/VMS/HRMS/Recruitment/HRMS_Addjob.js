(function () {
    'use strict';
    angular
        .module('app')
        .controller('vmsAddJobController', vmsAddJobController);

    vmsAddJobController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter'];
    function vmsAddJobController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter) {




        var paginationformasters;
        var copty;

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;

        // form Object
        $scope.EarningDet = {};
        $scope.addjob = {};
        $scope.mrfReq = {};
        $scope.addjob.hrjD_Id = 0;
        $scope.submitted = true;
       

        // Datatable display
        $scope.gridOptions = {
            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                //{ name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrmpeD_ED_Name', displayName: 'Earning' },
                { name: 'hrmpeD_PC_Amount_Flag', displayName: 'Amount/Per' },
                { name: 'hreeD_ED_Amount', displayName: 'Amount' },
                { name: 'hreeD_Percentage', displayName: 'Percentage' },
                { name: 'hreeD_PercentageOf_Name', displayName: '% of' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                        '<a ng-if="row.entity.hreeD_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);"> Activate</a>' +
                        '<span ng-if="row.entity.hreeD_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);">  Deactivate</a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }

        };

        $scope.deductiongridOptions = {
            enableFiltering: true,
            enableColumnMenus: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                //{ name: 'SlNo', field: 'name', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'hrmpeD_ED_Name', displayName: 'Deductions' },
                { name: 'hrmpeD_PC_Amount_Flag', displayName: 'Amount/Per' },
                { name: 'hreeD_ED_Amount', displayName: 'Amount' },
                { name: 'hreeD_Percentage', displayName: 'Percentage' },
                { name: 'hreeD_PercentageOf_Name', displayName: '% of' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableColumnMenu: false, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.EditData(row.entity);"> <i class="fa fa-pencil-square-o" style="color:blue;" aria-hidden="true"></i></a>' +
                        '<a ng-if="row.entity.hreeD_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.DeletRecord(row.entity);"> Activate</a>' +
                        '<span ng-if="row.entity.hreeD_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.DeletRecord(row.entity);">  Deactivate</a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }

        };


        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("AddJobVMS/getalldetails", pageid).then(function (promise) {

                if (promise.masterPositionList !== null && promise.masterPositionList.length > 0) {
                    $scope.masterPositionList = promise.masterPositionList;
                }

                if (promise.masterLocation !== null && promise.masterLocation.length > 0) {
                    $scope.masterLocation = promise.masterLocation;
                }

                if (promise.clientlist !== null && promise.clientlist.length > 0) {
                    $scope.clientlist = promise.clientlist;
                }

                if (promise.masterDepartmentList !== null && promise.masterDepartmentList.length > 0) {
                    $scope.masterDepartmentList = promise.masterDepartmentList;
                }

                if (promise.masterPriorityList !== null && promise.masterPriorityList.length > 0) {
                    $scope.masterPriorityList = promise.masterPriorityList;
                }

                if (promise.masterPosTypeList !== null && promise.masterPosTypeList.length > 0) {
                    $scope.masterPosTypeList = promise.masterPosTypeList;
                }

                if (promise.masterQualification !== null && promise.masterQualification.length > 0) {
                    $scope.masterQualification = promise.masterQualification;
                }

                if (promise.masterGender !== null && promise.masterGender.length > 0) {
                    $scope.masterGender = promise.masterGender;
                }
                if (promise.countrylist !== null && promise.countrylist.length > 0) {
                    $scope.countrylist = promise.countrylist;
                   
                    angular.forEach($scope.countrylist, function (head) {
                        if (head.ivrmmC_Default == 1) {
                            $scope.mrfReq.ivrmmC_Id = head.ivrmmC_Id;
                            $scope.staterecord()
                        }

                    })
                }

                if (promise.gridrecordMRF !== null && promise.gridrecordMRF.length > 0) {
                    $scope.gridrecordMRF = promise.gridrecordMRF;
                }



                if (promise.hrmrfR_Id !== 0) {
                    $scope.mrfReq = promise.vmsmrfList[0];
                }
               
            });

        };

        // clear form data
        $scope.comment = function (user) {
            $scope.hrmrfR_MDComment = user.hrmrfR_MDComment
            $scope.hrmrfR_UpdatedBy = user.hrmrfR_UpdatedBy
            $scope.PayScaleFrom = user.hrmrfR_PayScaleFrom
            $scope.PayScaleTo = user.hrmrfR_PayScaleTo
            $('#approvedcomment').modal('show');
        };

        $scope.cancel = function () {
            $scope.mrfReq = {};
            $state.reload();
        };
        $scope.staterecord = function (ivrmmC_Id) {
            $scope.statelist = [];
            var dataED = {
                "IVRMMC_Id": $scope.mrfReq.ivrmmC_Id,
            };

            apiService.create("AddJobVMS/staterecord", dataED).then(function (promise) {
                    if (promise.statelist !== null && promise.statelist.length > 0) {
                        $scope.statelist = promise.statelist;
                    }
                });
        };


        $scope.editrecord = function (user) {
            $scope.mrfReq.hrmeD_Id = user.hrmeD_Id;
            $scope.mrfReq.hrmP_Id = user.hrmP_Id;
            $scope.mrfReq.hrmrfR_Id = user.hrmrfR_Id;
            $scope.mrfReq.hrmpT_Id = user.hrmpT_Id;
            $scope.mrfReq.hrmpR_Id = user.hrmpR_Id;
            $scope.mrfReq.hrmD_Id = user.hrmD_Id;
            $scope.mrfReq.ivrmmS_Id = user.ivrmmS_Id;
            $scope.mrfReq.ivrmmC_Id = user.ivrmmC_Id;
            $scope.mrfReq.hrmD_DepartmentName = user.hrmD_DepartmentName;
            $scope.mrfReq.hrmrfR_JobLocation = user.hrmrfR_JobLocation;
            $scope.mrfReq.hrmrfR_PayScaleTo = user.hrmrfR_PayScaleTo;
            $scope.mrfReq.hrmrfR_PayScaleFrom = user.hrmrfR_PayScaleFrom;
            $scope.mrfReq.hrmrfR_TechnicalInterviewFlg = user.hrmrfR_TechnicalInterviewFlg;
            $scope.mrfReq.hrmrfR_OnlineTestFlg = user.hrmrfR_OnlineTestFlg;
            $scope.mrfReq.hrmrfR_WrittenTestFlg = user.hrmrfR_WrittenTestFlg;
            $scope.mrfReq.hrmrfR_Remark = user.hrmrfR_Remark;
            $scope.mrfReq.hrmrfR_Reason = user.hrmrfR_Reason;
            $scope.mrfReq.hrmrfR_Gender = user.ivrmmG_GenderName;
            $scope.mrfReq.hrmrfR_Age = user.hrmrfR_Age;
            $scope.mrfReq.hrmrfR_ExpTo = user.hrmrfR_ExpTo;
            $scope.mrfReq.hrmrfR_ExpFrom = user.hrmrfR_ExpFrom;
            $scope.mrfReq.hrmrfR_JobDesc = user.hrmrfR_JobDesc;
            $scope.mrfReq.hrmrfR_Skills = user.hrmrfR_Skills;
            $scope.mrfReq.hrmrfR_NoofPosition = user.hrmrfR_NoofPosition;
            $scope.mrfReq.hrmrfR_Qualification = user.hrmC_QulaificationName;
            $scope.mrfReq.hrmpT_Name = user.hrmpT_Name;
            $scope.mrfReq.hrmP_Name = user.hrmP_Name;
            $scope.mrfReq.hrmP_Position = user.hrmP_Position;



        };


        //Delete 
        $scope.deletedataY = function (user, SweetAlert) {

            var data = {
                "HRMRFR_Id": user.hrmrfR_Id,                
            }

            swal({
                title: "Are you sure",
                text: "Do you want to delete record????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("AddJobVMS/deleterecord", data).then(function (promise) {
                            $scope.cancel();
                            if (promise.retrunMsg !== "") {

                                if (promise.retrunMsg === "Deleted") {
                                    swal("Record Deleted successfully");
                                    $state.reload();
                                }
                                else if (promise.retrunMsg === "Failed") {
                                    swal("Record Not Failed");
                                }
                                else {
                                    swal("Record Not Failed", 'Fail');
                                }

                                $scope.cancel();
                            }
                        });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };


        $scope.GetEarningDeductionList = function (hrmpeD_ED_Flag) {
            $scope.headList = [];
            var dataED = {
                "HRMPED_ED_Flag": hrmpeD_ED_Flag,
                "Type": "EarningDeductionList"
            };

            apiService.create("EmployeeEarningDeductionMap/", dataED).
                then(function (promise) {
                    if (promise.earningdetectionList !== null && promise.earningdetectionList.length > 0) {
                        $scope.headList = promise.earningdetectionList;
                    }
                });
        };

        $scope.sectioncheckboxchcked = [];

        $scope.submitted = false;
        $scope.saveData = function (data) {


            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                data.perc_OfDTO = [];

                angular.forEach($scope.listData, function (data, key) {
                    if (data.Selected) {
                        $scope.sectioncheckboxchcked.push(data);
                        console.log(data);
                    }
                })

                if (data.hrmpeD_PC_Amount_Flag === "Amount") {
                    data.hreeD_ED_Amount = $scope.AmountPercent;
                    data.hreeD_Percentage = "";
                }
                else if (data.hrmpeD_PC_Amount_Flag === "Percentage") {
                    data.hreeD_Percentage = $scope.AmountPercent;
                    data.hreeD_ED_Amount = "";

                    if ($scope.sectioncheckboxchcked == null || $scope.sectioncheckboxchcked.length == 0) {
                        swal('Kindly select atleast one record', 'Select Percentage Of')
                        return;
                    }
                }

                data.perc_OfDTO = $scope.sectioncheckboxchcked;

                apiService.create("EmployeeEarningDeductionMap/", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {
                            if (promise.retrunMsg == "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }
                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');
                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                                $state.reload();
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                                $state.reload();
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                            }

                            //if (promise.headList !== null && promise.headList.length > 0) {
                            //    //head list
                            //    $scope.headList = promise.headList;
                            //}

                            //if (promise.employeedetailList != null && promise.employeedetailList.length > 0) {
                            //    //employee list
                            //    $scope.employeedetailList = promise.employeedetailList;
                            //}

                            ////earning list
                            //if (promise.earningList != null && promise.earningList.length > 0) {
                            //    $scope.gridOptions.data = promise.earningList;
                            //}

                            //if (promise.detectionList != null && promise.detectionList.length > 0) {
                            //    //deduction
                            //    $scope.deductiongridOptions.data = promise.detectionList;
                            //}

                            ////earning/deduction list
                            //if (promise.earningdetectionList != null && promise.earningdetectionList.length > 0) {
                            //    //deduction
                            //    $scope.listData = promise.earningdetectionList;
                            //}
                            $scope.cancel();
                        }
                    });
            }

        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


        // Edit Single Record
        $scope.EditData = function (record) {
            var id = record.hrmrfR_Id;
            apiService.getURI("AddJobVMS/editRecord", id).
                then(function (promise) {




                });
        };

        $scope.SetEarningDeductionListByID = function (hrmpeD_ED_Flag, hrmpeD_Id) {

            $scope.headList = [];

            var dataED = {
                "HRMPED_ED_Flag": hrmpeD_ED_Flag,
                "Type": "EarningDeductionList"
            }

            apiService.create("EmployeeEarningDeductionMap/", dataED).
                then(function (promise) {

                    //earning/deduction list
                    if (promise.earningdetectionList != null && promise.earningdetectionList.length > 0) {
                        //deduction
                        $scope.headList = promise.earningdetectionList;
                        angular.forEach($scope.headList, function (head, key) {
                            if (head.hrmpeD_Id == hrmpeD_Id) {
                                head.Selected = true;
                                // console.log(head);
                                $scope.EarningDet.hrmpeD_Id = head.hrmpeD_Id.toString();
                            }

                        })
                    }

                })
        }

        $scope.headList = [];
        $scope.SetHeadName = function (hrmpeD_Id) {
            $scope.EarningDet.hrmpeD_ED_Name = "";
            if (hrmpeD_Id != "" && hrmpeD_Id != undefined) {

                angular.forEach($scope.headList, function (value, key) {
                    if (value.hrmpeD_Id === parseInt(hrmpeD_Id)) {

                        $scope.EarningDet.hrmpeD_ED_Name = value.hrmpeD_ED_Name;
                    }
                })
            }
        }

        //deactivate record
        $scope.DeletRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.hreeD_ActiveFlag == false) {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }

            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Earning and Deduction..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("EmployeeEarningDeductionMap/ActiveDeactiveRecord", data.hrmeD_Id).
                            then(function (promise) {


                                if (promise.retrunMsg !== "") {

                                    if (promise.retrunMsg === "Activated") {
                                        swal("Record Activated successfully");
                                    }
                                    else if (promise.retrunMsg === "Deactivated") {
                                        swal("Record Deactivated successfully");
                                    }
                                    else {
                                        swal("Record Not Activated/Deactivated", 'Fail');
                                    }

                                    $scope.cancel();
                                }

                            })
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }

            );
        }

        $scope.listDataDis = false;
        $scope.AmountPercentDis = true;
        $scope.AmountPertcentLabel = "Amount/Percentage";
        $scope.setAmountPercentLable = function (da) {

            $scope.AmountPercentDis = false;
            if (da == "Amount") {
                $scope.AmountPertcentLabel = "Amount";
                $scope.listDataDis = false;
            }
            else if (da == "Percentage") {
                $scope.AmountPertcentLabel = "Percentage";
                $scope.listDataDis = true;
            }
        };


        $scope.savejob = function () {
            if ($scope.myForm.$valid) {
                $scope.mrfReq.hrmrfR_ManagerFlag = true;
                $scope.mrfReq.hrmrfR_Status = "Pending";
                if ($scope.mrfReq.hrmrfR_JobLocation === "Location") {
                    $scope.mrfReq.hrmrfR_JobLocation = $scope.mrfReq.dynamicloc;
                }
                if ($scope.mrfReq.hrmrfR_JobLocation === "Client") {
                    $scope.mrfReq.hrmrfR_JobLocation = $scope.mrfReq.clientlocation;
                }
                var data = $scope.mrfReq;
                apiService.create("AddJobVMS/", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {

                            if (promise.retrunMsg === "Duplicate") {
                                swal("Record already exist..!!");
                                return;
                            }

                            else if (promise.retrunMsg === "false") {
                                swal("Record Not saved / Updated..", 'Fail');

                            }
                            else if (promise.retrunMsg === "Add") {
                                swal("Record Saved Successfully..");
                                $state.reload();
                            }
                            else if (promise.retrunMsg === "Update") {
                                swal("Record Updated Successfully..");
                                $state.reload();
                            }
                            else {
                                swal("Something went wrong ..!", 'Kindly contact Administrator');
                            }

                            if (promise.employeeTypeList !== null && promise.employeeTypeList.length > 0) {
                                $scope.gridOptionsEmployeeType.data = promise.employeeTypeList;
                            }
                            $scope.cancel();
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        //$scope.SelectedFileForUploadzd = [];
        //$scope.selectFileforUploadzd = function (input, document) {
        //    $('#' + document.id).removeAttr('src');
        //    $scope.SelectedFileForUploadzd = input.files;
        //    if (input.files && input.files[0]) {
        //        var filename = input.files[0].name.toString();
        //        var nameArray = filename.split('.');
        //        var extention = nameArray[nameArray.length - 1];
        //        if ((extention === "JPEG" || extention === "jpg" || extention === "docx" || extention === "doc" || extention === "pdf") && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
        //        {
        //            var reader = new FileReader();
        //            reader.onload = function (e) {
        //                $('#' + document.id) //hrmedS_Id
        //                    .attr('src', e.target.result);
        //            };
        //            reader.readAsDataURL(input.files[0]);
        //            UploadEmployeeDocument(document);
        //        }
        //        else if (extention === "JPEG" && extention !== "jpg" && extention !== "docx" && extention !== "doc" && extention !== "pdf") {
        //            $('#' + document.id).removeAttr('src');
        //            swal("Please Upload the valid file");
        //            return;
        //        } else if (input.files[0].size > 2097152) {
        //            $('#' + document.id).removeAttr('src');
        //            swal("Document size should be less than 2MB");
        //            return;
        //        }
        //    }
        //};


        $scope.SelectedFileForUploadzdd = [];
        $scope.selectFileforUploadzd = function (input, document) {

            $scope.SelectedFileForUploadzdd = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.ismresgmcL_Id).attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofiled(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.ismresgmcL_Id).attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofiled(document);
                }
                else if (input.files[0].type === "application/msword") {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#' + document.ismresgmcL_Id).attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofiled(document);
                }

                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload MP4, Pdf, Image Files Only");
                }
            }
        };

        $scope.showmodaldetailscheck = function (data) {
            $('#preview').removeAttr('src');
            var filename = data.toString();
            var nameArray = filename.split('.');
            var extention = nameArray[nameArray.length - 1];

            if (extention == "jpg" || extention == "jpeg" || extention == "png") {
                $('#preview').attr('src', data);
                $('#myModal').modal('show');
            }
            else if (extention == "pdf" || extention == "xlsx" || extention == "ppt" || extention == "zip" || extention == "doc" || extention == "docx" || extention == "txt") {
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

        function UploadEmployeeDocument(data) {
            // console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzd.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzd[i]);
            }
            // We can send more data to server using append         
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadnaacdocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    if (d !== "PathNotFound") {
                        data.hrmrfR_Attachment = d;
                    } else {
                        swal('Document Storage Path Not Found ..!!');
                    }
                })
                .error(function () {
                    $('#' + data.id).removeAttr('src');
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.getposition = function (data) {
            angular.forEach($scope.masterPositionList, function (value, key) {
                if (value.hrmP_Id === parseInt(data)) {
                    $scope.mrfReq.hrmrfR_Skills = value.hrmP_Skills;
                    $scope.mrfReq.hrmrfR_JobDesc = value.hrmP_Desc;
                }
            });
        };
    }

})();