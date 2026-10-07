(function () {
    'use strict';
    angular.module('app').controller('PC_RequisitionController', PC_RequisitionController)

    PC_RequisitionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function PC_RequisitionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

        $scope.getserverdate = function () {
            var xmlHttp;
            function srvTime() {
                try {
                    //FF, Opera, Safari, Chrome
                    xmlHttp = new XMLHttpRequest();
                }
                catch (err1) {
                    //IE
                    try {
                        xmlHttp = new ActiveXObject('Msxml2.XMLHTTP');
                    }
                    catch (err2) {
                        try {
                            xmlHttp = new ActiveXObject('Microsoft.XMLHTTP');
                        }
                        catch (eerr3) {
                            //AJAX not supported, use CPU time.
                            alert("AJAX not supported");
                        }
                    }
                }
                xmlHttp.open('HEAD', window.location.href.toString(), false);
                xmlHttp.setRequestHeader("Content-Type", "text/html");
                xmlHttp.send('');
                return xmlHttp.getResponseHeader("Date");
            }
            $scope.today = srvTime();
        }

        $scope.getserverdate();

        $scope.submitted = false;
        $scope.totalgrid = [];
        $scope.PCREQTN_Date = new Date($scope.today);
        $scope.editflag = false;
        $scope.maxdate = new Date($scope.today);
        $scope.editadd = false;

        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";

            var pageid = 2;
            apiService.getURI("PC_Requisition/onloaddata", pageid).then(function (promise) {
                $scope.getdept = promise.getdept;
                $scope.getparticulars = promise.getparticulars;
                $scope.getloaddata = promise.getloaddata;
                $scope.getapprovedindent = promise.getapprovedindent;
                $scope.getapprovedindentcashapproved = promise.getapprovedindentcashapproved;

                angular.forEach($scope.getloaddata, function (d) {
                    d.approvedflag = false;
                    angular.forEach($scope.getapprovedindent, function (dd) {
                        if (d.pcreqtN_Id === dd.pcreqtN_Id) {
                            d.approvedflag = true;
                        }
                    });
                });

                angular.forEach($scope.getloaddata, function (d) {
                    d.approvedindentcashflag = false;
                    angular.forEach($scope.getapprovedindentcashapproved, function (dd) {
                        if (d.pcreqtN_Id === dd.pcreqtN_Id) {
                            d.approvedindentcashflag = true;
                        }
                    });
                });

            });
        };

        $scope.onchangedept = function () {
            $scope.totalgrid = [];
            $scope.totalgrid = [{ id: 'pcreq1' }];
            var data = {
                "HRMD_Id": $scope.HRMD_Id.hrmD_Id
            };
            apiService.create("PC_Requisition/onchangedept", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getemp = promise.getemp;

                }

            });
        };

        $scope.onchangeparticuar = function (user, index) {
            for (var k = 0; k < $scope.totalgrid.length; k++) {
                var roll = parseInt(user.pcmparT_Id);
                var arryind = $scope.totalgrid.indexOf($scope.totalgrid[k]);
                console.log(arryind);
                if (arryind !== index) {
                    var rollindex = parseInt($scope.totalgrid[k].PCMPART_Id.pcmparT_Id);
                    if (rollindex === roll) {
                        swal("Particular Already Exists");
                        $scope.totalgrid[index].PCMPART_Id = "";
                        $scope.totalgrid[index].amount = "";
                        $scope.totalgrid[index].remarks = "";
                        break;
                    }
                }
            }
        };

        $scope.onchangeamount = function (userobj) {
            $scope.PCREQTN_TotAmount = 0.00;
            $scope.PCREQTN_TotAmounttemp = 0.00;

            angular.forEach($scope.totalgrid, function (dd) {
                if (dd.amount !== undefined && dd.amount !== null && dd.amount !== "") {
                    $scope.PCREQTN_TotAmount = $scope.PCREQTN_TotAmount + parseFloat(dd.amount);
                    $scope.PCREQTN_TotAmounttemp = $scope.PCREQTN_TotAmounttemp + parseFloat(dd.amount);
                }
            });
        };

        $scope.saverecord = function () {
            if ($scope.myForm.$valid) {

                $scope.savePC_Requisition_DetailsDTO = [];

                angular.forEach($scope.totalgrid, function (ddd) {
                    if (ddd.remarks === undefined || ddd.remarks === null || ddd.remarks === "") {
                        ddd.remarks = "";
                    }
                    $scope.savePC_Requisition_DetailsDTO.push({
                        PCMPART_Id: ddd.PCMPART_Id.pcmparT_Id,
                        PCMPART_ParticularName: ddd.PCMPART_Id.pcmparT_ParticularName,
                        PCREQTNDET_Amount: parseFloat(ddd.amount), PCREQTNDET_Remarks: ddd.remarks, PCREQTNDET_Id: ddd.pcreqtndeT_Id
                    });

                });

                var data = {
                    "PCREQTN_Id": $scope.PCREQTN_Id,
                    "HRMD_Id": $scope.HRMD_Id.hrmD_Id,
                    "HRME_Id": $scope.HRME_Id.hrmE_Id,
                    "PCREQTN_Purpose": $scope.PCREQTN_Purpose,
                    "PCREQTN_TotAmount": $scope.PCREQTN_TotAmount,
                    "PCREQTN_Date": new Date($scope.PCREQTN_Date).toDateString(),
                    "PC_Requisition_DetailsDTO": $scope.savePC_Requisition_DetailsDTO
                };

                apiService.create("PC_Requisition/saverecord", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message === "Duplicate") {
                            swal("Record Already Exists");
                        } else if (promise.message === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.message === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else if (promise.message === "Error") {
                            swal("Failed To Save/ Update Record");
                        } else {
                            swal("Something Went Wrong Contact Administrator");
                        }
                        $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.EditData = function (user) {
            var data = {
                "PCREQTN_Id": user.pcreqtN_Id
            };
            apiService.create("PC_Requisition/EditData", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.geteditdetails !== null && promise.geteditdetails.length > 0) {
                        $scope.geteditdept = promise.geteditdept;
                        $scope.HRMD_Id = $scope.geteditdept[0];

                        $scope.getemp = promise.geteditemp;
                        $scope.geteditsavedemp = promise.geteditsavedemp;
                        $scope.HRME_Id = $scope.geteditsavedemp[0];

                        $scope.PCREQTN_Purpose = promise.geteditdetails[0].pcreqtN_Purpose;
                        $scope.PCREQTN_Date = new Date(promise.geteditdetails[0].pcreqtN_Date);
                        $scope.PCREQTN_TotAmount = promise.geteditdetails[0].pcreqtN_TotAmount;
                        $scope.PCREQTN_TotAmounttemp = promise.geteditdetails[0].pcreqtN_TotAmount;
                        $scope.PCREQTN_Id = promise.geteditdetails[0].pcreqtN_Id;

                        $scope.geteditsavedparticulars = promise.geteditsavedparticulars;

                        $scope.totalgrid = [];

                        angular.forEach($scope.geteditsavedparticulars, function (dd) {
                            $scope.totalgrid.push({
                                PCMPART_Id: dd, amount: dd.pcreqtndeT_Amount, remarks: dd.pcreqtndeT_Remarks,
                                pcreqtndeT_Id: dd.pcreqtndeT_Id, editfalguser: true, editadduser: true
                            });
                        });

                        $scope.editflag = true;
                        $scope.editadd = true;
                    }
                }

            });
        };

        $scope.deactiveY = function (user, SweetAlert) {


            var data = {
                "PCREQTN_Id": user.pcreqtN_Id
            };

            var dystring = "";
            if (user.pcreqtN_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (user.pcreqtN_ActiveFlg === false) {
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
                        apiService.create("PC_Requisition/deactiveY", data).then(function (promise) {
                            if (promise.message === "Mapped") {
                                swal("Record Is Already Mapped, So You Can Not Deactive The Record");
                            } else {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            }
                        });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }
                });
        };

        $scope.Viewdata = function (user) {
            $scope.PCREQTN_RequisitionNotemp = user.pcreqtN_RequisitionNo;
            $scope.viewapprovedflag = user.approvedflag;
            var data = {
                "PCREQTN_Id": user.pcreqtN_Id,
                "PCMPART_Id": user.pcmparT_Id
            };
            apiService.create("PC_Requisition/Viewdata", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getviewdata = promise.getviewdata;
                    $scope.modalview = promise.modalview;
                    if (promise.modalview != null && promise.modalview.length > 0) {

                        if (promise.modalview[0].First_Approvalperson != null && promise.modalview[0].Second_Approvalperson == null) {
                            $scope.status = "Paritally Approved";
                        }
                        else if (promise.modalview[0].Second_Approvalperson != null && promise.modalview[0].First_Approvalperson != null) {
                            $scope.status = "Approved";
                        }

                    }
                    else {
                        $scope.status = "Pending";
                    }
                }

            });
        };



        $scope.ApprovalRemark = function (user) {
            $scope.PCREQTN_RequisitionNotemp = user.pcreqtN_RequisitionNo;

            var data = {
                "PCREQTN_Id": user.pcreqtN_Id,
                "PCMPART_Id": user.pcmparT_Id
            };
            apiService.create("PC_Requisition/Viewdata", data).then(function (promise) {
                if (promise !== null && promise.modalview.length > 0) {
                    $scope.modalview = promise.modalview;

                }
                else {

                    swal(" First Level is Not Approved  !");

                    $('#mymodalviewdetailssecondtab').modal('hide');
                    
                }
                
            });
           
        };




        $scope.deactiveparticulars = function (user, SweetAlert) {
            var data = {
                "pcreqtndeT_Id": user.pcreqtndeT_Id,
                "PCREQTN_Id": user.pcreqtN_Id
            };

            var dystring = "";
            if (user.pcreqtndeT_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (user.pcreqtndeT_ActiveFlg === false) {
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
                        apiService.create("PC_Requisition/deactiveparticulars", data).then(function (promise) {
                            if (promise.message === "Mapped") {
                                swal("Record Is Already Mapped, So You Can Not Deactive The Record");
                            } else {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $scope.getviewdata = promise.getviewdata;
                            }
                        });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }
                });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.totalgrid = [{ id: 'pcreq1' }];

        $scope.addNewsiblingguard = function () {
            var newItemNo = $scope.totalgrid.length + 1;
            if (newItemNo <= 50) {
                $scope.totalgrid.push({ 'id': 'pcreq' + newItemNo, editfalguser: false, editadduser: false });
            }
            console.log($scope.totalgrid);
            if ($scope.editflag === true) {
                $scope.editadd = false;
            }
        };

        $scope.removeNewsiblingguard = function (index) {
            var newItemNo = $scope.totalgrid.length - 1;
            $scope.totalgrid.splice(index, 1);

            if ($scope.totalgrid.length === 0) {
                //data
            }

            $scope.PCREQTN_TotAmount = 0.00;
            $scope.PCREQTN_TotAmounttemp = 0.00;
            angular.forEach($scope.totalgrid, function (dd) {
                if (dd.amount !== undefined && dd.amount !== null && dd.amount !== "") {
                    $scope.PCREQTN_TotAmount = $scope.PCREQTN_TotAmount + parseFloat(dd.amount);
                    $scope.PCREQTN_TotAmounttemp = $scope.PCREQTN_TotAmounttemp + parseFloat(dd.amount);
                }
            });
        };

        $scope.search = '';

        $scope.filterValue1 = function (obj) {
            return ($filter('date')(obj.pcreqtN_Date, 'dd/MM/yyyy').indexOf($scope.search) >= 0) ||
                (angular.lowercase(obj.employeename)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.pcreqtN_RequisitionNo)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.departmentname)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.pcreqtN_Purpose)).indexOf(angular.lowercase($scope.search)) >= 0;
        };
    }
})();

