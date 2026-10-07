
(function () {
    'use strict';
    angular
        .module('app')
        .controller('OrderApprovalController', OrderApprovalController);
    OrderApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache','$compile'];
    function OrderApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $compile) {

        var date = new Date();
        $scope.invmpI_PIDate = date;
        $scope.apflag = false;
        $scope.editS = false;
        $scope.printbill = false;
        $scope.obj = {};
        $scope.pidetails = [];
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

        //=====================Adding and removing new row in transcation Grid============      
        $scope.transrows = [{ itrS_Id: 'trans1' }];
        if ($scope.transrows.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addpirows = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.transrows.length > 1) {
                    for (var i = 0; i === $scope.transrows.length; i++) {
                        var id = $scope.transrows[i].itrS_Id;
                        var lastChar = id.substr(id.length - 1);
                        $scope.cnt = parseInt(lastChar);
                    }
                }
                $scope.cnt = $scope.cnt + 1;
                $scope.tet = 'trans' + $scope.cnt;
                var newItemNo = $scope.cnt;
                $scope.transrows.push({ 'itrS_Id': 'trans' + newItemNo });
            }
            else {
                $scope.submitted = true;
            }

        };
        $scope.removepirows = function (index, data) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);

            if (data.amstB_Id > 0) {
                $scope.Deletepirows(data);
            }

        };


        $scope.removeitemss = function (index, data) {
            var a = 0;
            $scope.newindent_list.splice(index, 1);

            $scope.rejindent_list.push(data);
            angular.forEach($scope.newindent_list, function (obj) {
                a += parseFloat(obj.invtpI_ApproxAmount);
            });
            var totamt = a;
            $scope.invmpI_ApproxTotAmount = totamt;
            $scope.invmpI_ApproxTotAmount = parseFloat($scope.invmpI_ApproxTotAmount);
            $scope.invmpI_ApproxTotAmount = $scope.invmpI_ApproxTotAmount.toFixed(2);
            console.log($scope.rejindent_list);
            console.log($scope.newindent_list);

        };


        //====================== Auto Generated Number
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                if (transnumconfigsettings[i].imN_Flag === "INVPI") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }

        //====================================== Page Load
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;

            $scope.currentPage2 = 1;
            $scope.itemsPerPage2 = 10;

            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;
            $scope.pppfl = true;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("OrderApproval/getloaddata", pageid).
                then(function (promise) {
                    $scope.prflag = true;
                    $scope.get_purchaseindent = promise.get_prNo;
                    $scope.get_item = promise.get_item;
                    //$scope.get_purchaseindent = promise.get_purchaseindent;
                    $scope.presentCountgrid = $scope.get_purchaseindent.length;
                    if ($scope.get_purchaseindent.length>0) {
                        $scope.prflag = true;
                        $scope.pppfl = false;
                    }
                    else {
                        $scope.pppfl = true;
                    }
                    //else {
                    //    $scope.prflag = false;
                    //}
                    //$scope.ap_list = [];
                    //angular.forEach($scope.get_purchaseindent, function (pino) {
                    //    angular.forEach(promise.get_prNo, function (prno) {
                    //        if (pino.invmpR_Id == prno.invmpR_Id) {
                    //            $scope.apflag = true;
                    //        }
                    //        else {
                    //            $scope.apflag = false;
                    //        } 
                    //        $scope.ap_list.push({
                    //            'invmpR_Id': prno.invmpR_Id, 'hrmE_Id': prno.hrmE_Id, 'employeename': prno.employeename, 'invmpR_ApproxTotAmount': prno.invmpR_ApproxTotAmount, 'invmpR_PRDate': prno.invmpR_PRDate, 'apflag': $scope.apflag
                    //        });
                    //    })
                    //})

                    //$scope.ap_list = [];
                    //angular.forEach(promise.get_prNo, function (prno) {
                    //    angular.forEach($scope.get_purchaseindent, function (pino) {

                    //        if (pino.invmpR_Id == prno.invmpR_Id) {
                    //            $scope.apflag = true;
                    //        }
                    //        else {
                    //            $scope.apflag = false;
                    //        }
                    //        $scope.ap_list.push({
                    //            'invmpR_Id': prno.invmpR_Id, 'hrmE_Id': prno.hrmE_Id, 'employeename': prno.employeename, 'invmpR_ApproxTotAmount': prno.invmpR_ApproxTotAmount, 'invmpR_PRDate': prno.invmpR_PRDate, 'apflag': $scope.apflag
                    //        });

                    //    })
                    //})




                });
        };

        $scope.newinvmprid = 0;
        $scope.rejflg = false;
        //======================================== View PR
        $('#myModal').modal('hide');
        $scope.viewtx = function (objc) {
            $scope.all1 = false;
            $scope.rejflg = false;
            $scope.addcrtshow = false;
            var invmprid = objc.invmpR_Id;
            $scope.newinvmprid = objc.invmpR_Id;
            var data = {
                "INVMPR_Id": invmprid
             
            };
            apiService.create("OrderApproval/getprDetail", data).
                then(function (promise) {
                    $scope.get_indentDetail = promise.get_indentDetail;
                    $scope.invmpI_ApproxTotAmount = $scope.get_indentDetail[0].invmpR_ApproxTotAmount;
                    $scope.prno = $scope.get_indentDetail[0].invmpR_PRNo;
                    $scope.indent_list = [];
                    angular.forEach($scope.get_indentDetail, function (idn) {
                        $scope.indent_list.push({
                            'invmpR_Id': idn.invmpR_Id, 'invtpR_Id': idn.invtpR_Id, 'invmI_Id': idn.invmI_Id, 'invmuoM_Id': idn.invmuoM_Id, invmpR_PRNo: idn.invmpR_PRNo,
                            'hrmE_Id': idn.hrmE_Id, 'invmI_ItemName': idn.invmI_ItemName, 'invmI_ItemCode': idn.invmI_ItemCode, 'invmuoM_UOMName': idn.invmuoM_UOMName,
                            'employeename': idn.employeename, 'invtpR_PRQty': idn.invtpR_PRQty, 'invtpI_PIQty': idn.invtpR_PRQty, 'invtpR_PRUnitRate': idn.invtpR_PRUnitRate, 'invtpI_PIUnitRate': idn.invtpR_PRUnitRate, 'invtpR_ApproxAmount': idn.invtpR_ApproxAmount, 'invmpR_ApproxTotAmount': idn.invmpR_ApproxTotAmount, 'invtpI_ApproxAmount': idn.invtpR_ApproxAmount, 'invtpR_ApprovedQty': idn.invtpR_ApprovedQty, 'invmpR_PRDate': idn.invmpR_PRDate, 'invmpR_Remarks': idn.invmpR_Remarks, 'invmpI_ApproxTotAmount': idn.invmpR_ApproxTotAmount, 'invtpR_ActiveFlg': idn.invtpR_ActiveFlg, 'invmpR_PICreatedFlg': idn.invmpR_PICreatedFlg
                        });
                    });
                    $('#myModal').modal('show');
                });
        };
        ////====================================== Add To Cart

        $scope.finalindent_list = [];
        $scope.newindent_list = [];
        $scope.rejindent_list = [];
        $scope.addtocart = function (objtx) {
            var a = 0;
          
            angular.forEach($scope.indent_list, function (oobj) {
               

                if (oobj.checkedvalue === true) {
                    var cnt = 0;
                    angular.forEach($scope.rejindent_list, function (dd, index) {
                        if (oobj.invtpR_Id === dd.invtpR_Id) {

                            $scope.rejindent_list.splice(index, 1);

                        }
                    });
                    angular.forEach($scope.newindent_list, function (obj) {
                        if (oobj.invtpR_Id === obj.invtpR_Id) {
                            cnt += 1;

                          

                           
                        }
                    });
                    if (cnt===0) {
                        $scope.newindent_list.push(oobj);
                    }

                    
                }
                else {

                    var cnt1 = 0;

                    angular.forEach($scope.rejindent_list, function (obj) {
                        if (oobj.invtpR_Id === obj.invtpR_Id) {
                            cnt1 += 1;
                            
                        }
                    });

                    if (cnt1 === 0) {
                        var cnt2 = 0;
                        angular.forEach($scope.newindent_list, function (obj) {
                            if (oobj.invtpR_Id === obj.invtpR_Id) {
                                cnt2 += 1;

                            }
                        });

                        if (cnt2===0) {
                            $scope.rejindent_list.push(oobj);
                        }

                        
                    }
                   
                }
            
            });

            console.log($scope.newindent_list);
            console.log($scope.rejindent_list);
            angular.forEach($scope.newindent_list, function (obj) {
                a += parseFloat(obj.invtpI_ApproxAmount);
            });
            var totamt = a;
            $scope.invmpI_ApproxTotAmount = totamt;
            $scope.invmpI_ApproxTotAmount = parseFloat($scope.invmpI_ApproxTotAmount);
            $scope.invmpI_ApproxTotAmount = $scope.invmpI_ApproxTotAmount.toFixed(2);
        };


        //====================================== On Item Change
        $scope.onitemchange = function (itemid, objid) {
            var item_id = itemid.invmI_Id;
            var data = {
                "INVMI_Id": item_id
            };
            apiService.create("INV_PurchaseRequisition/getitemDetail", data).
                then(function (promise) {
                    $scope.get_itemDetail = promise.get_itemDetail;
                    angular.forEach($scope.transrows, function (obj) {
                        if (obj.itrS_Id === objid.itrS_Id) {
                            obj.invmuoM_UOMName = $scope.get_itemDetail[0].invmuoM_UOMName;
                            obj.invstO_AvaiableStock = $scope.get_itemDetail[0].invstO_AvaiableStock;
                        }
                    });
                });
        };
        //================== Count Amount
        $scope.countitemAmt = function (rowobj) {

            if (rowobj.flag==='A') {
                var a = 0;
                $scope.purrate = parseFloat(rowobj.INVTPO_RatePerUnit);
                $scope.qty = parseFloat(rowobj.INVTPIAPP_ApprovedQty);
                rowobj.invtpI_ApproxAmount = $scope.purrate * $scope.qty;
                if ($scope.editS === true) {
                    angular.forEach($scope.pidetails, function (obj) {

                        if (obj.flag==='A') {
                            a += parseFloat(obj.invtpI_ApproxAmount);
                        }
                       
                    });
                }
                else {
                    angular.forEach($scope.transrows, function (obj) {
                        a += parseFloat(obj.invtpI_ApproxAmount);
                    });
                }
                var totamt = a;
                $scope.invmpI_ApproxTotAmount = totamt;
                $scope.invmpI_ApproxTotAmount = parseFloat($scope.invmpI_ApproxTotAmount);
                $scope.invmpI_ApproxTotAmount = $scope.invmpI_ApproxTotAmount.toFixed(2);
            }
          
        };

        //================== Count Amount rate
        $scope.countitemAmtQ = function (rowobj) {
            if (rowobj.flag==='A') {

          

            var a = 0;
                $scope.purrate = parseFloat(rowobj.INVTPO_RatePerUnit);
                $scope.qty = parseFloat(rowobj.INVTPO_ApproveQty);
                rowobj.INVTPO_Amount = $scope.purrate * $scope.qty;
            if ($scope.editS === true) {
                angular.forEach($scope.pidetails, function (obj) {
                    if (obj.flag === 'A') {
                        a += parseFloat(obj.INVTPO_Amount);
                    }
                   
                });
            }
            else {
                angular.forEach($scope.transrows, function (obj) {
                    a += parseFloat(obj.INVTPO_Amount);
                });
            }
            var totamt = a;
                $scope.INVMPO_TotAmount = totamt;
                $scope.INVMPO_TotAmount = parseFloat($scope.INVMPO_TotAmount);
                $scope.INVMPO_TotAmount = $scope.INVMPO_TotAmount.toFixed(2);

            }
        };

        //===================================== PR Change
        //$scope.onprchange = function (itemid) {
        //    $scope.get_indentDetail = "";
        //    var item_id = itemid.invmpR_Id;
        //    var data = {
        //        "INVMPR_Id": item_id
        //    }
        //    apiService.create("OrderApproval/getprDetail", data).
        //        then(function (promise) {
        //            $scope.get_indentDetail = promise.get_indentDetail;
        //            $scope.invmpI_ApproxTotAmount = $scope.get_indentDetail[0].invmpR_ApproxTotAmount;
        //            $scope.indent_list = [];
        //            angular.forEach($scope.get_indentDetail, function (idn) {
        //                $scope.indent_list.push({
        //                    invmpR_Id: idn.invmpR_Id, invtpR_Id: idn.invtpR_Id, invmI_Id: idn.invmI_Id, invmuoM_Id: idn.invmuoM_Id,
        //                    hrmE_Id: idn.hrmE_Id, invmI_ItemName: idn.invmI_ItemName, invmI_ItemCode: idn.invmI_ItemCode, invmuoM_UOMName: idn.invmuoM_UOMName,
        //                    employeename: idn.employeename, invtpR_PRQty: idn.invtpR_PRQty, invtpI_PIQty: idn.invtpR_PRQty, invtpR_PRUnitRate: idn.invtpR_PRUnitRate, invtpI_PIUnitRate: idn.invtpR_PRUnitRate, invtpR_ApproxAmount: idn.invtpR_ApproxAmount, invmpR_ApproxTotAmount: idn.invmpR_ApproxTotAmount, invtpI_ApproxAmount: idn.invtpR_ApproxAmount, invtpR_ApprovedQty: idn.invtpR_ApprovedQty, invmpR_PRDate: idn.invmpR_PRDate, invmpR_Remarks: idn.invmpR_Remarks, invmpI_ApproxTotAmount: idn.invmpR_ApproxTotAmount, invtpR_ActiveFlg: idn.invtpR_ActiveFlg, invmpR_PICreatedFlg: idn.invmpR_PICreatedFlg
        //                });
        //            })
        //        })
        //}

        //================== Count Amount
        $scope.countAQAmt = function (piobj) {
            var a = 0;
            $scope.purrate = parseFloat(piobj.INVTPO_RatePerUnit);
            $scope.qty = parseFloat(piobj.invtpI_PIQty);
            piobj.invtpI_ApproxAmount = $scope.purrate * $scope.qty;
            angular.forEach($scope.indent_list, function (obj) {
                a += parseFloat(obj.invtpI_ApproxAmount);
            });
            var totamt = a;
            $scope.invmpI_ApproxTotAmount = totamt;
            $scope.invmpI_ApproxTotAmount = parseFloat($scope.invmpI_ApproxTotAmount);
            $scope.invmpI_ApproxTotAmount = $scope.invmpI_ApproxTotAmount.toFixed(2);
        };

        //===================================== SAVE DATA

       
        var approvecnt = 0;
        $scope.savedata = function () {
            approvecnt = 0;
            $scope.submitted = true;
            var data = {};
            if ($scope.myForm.$valid) {
                $scope.arrayPI = [];
                $scope.arrayPIr = [];
                angular.forEach($scope.pidetails, function (li) {
                    if (li.flag==='A') {
                        approvecnt += 1;
                    }
                    });

                console.log($scope.pidetails);
            
                    data = {
                        "INVTPO_Remarks": $scope.INVTPO_Remarks,
                        "INVMPO_TotAmount": $scope.INVMPO_TotAmount,
                        "INVMPO_TotRate": $scope.invmpO_TotRate,
                        "INVMPO_TotTax": $scope.invmpO_TotTax,
                        "arrayPI": $scope.pidetails,
                        "INVMPO_Id": $scope.INVMPO_Id,
                        "MI_Id": $scope.MI_Id,
                        "approvecnt": approvecnt,
                    };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("OrderApproval/savedetails", data).then(function (promise) {

                    if (promise.returnval === true) {
                            swal('Purchase Order Approved/Rejected successfully');
                    }
                    else {
                        if (promise.invmpI_Id === 0 || promise.invmpI_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmpI_Id > 0) {
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

        //============================================ Print Receipt
        $scope.toggleAll = function (pial) {
            $scope.piall = pial;
            $scope.printbill = false;
            var toggleStatus = $scope.piall;
            angular.forEach($scope.get_purchaseindent, function (pialll) {
                pialll.checkedvalue = toggleStatus;
                $scope.printbill = true;
            });
        };

        $scope.toggleAll1 = function (pial) {
            $scope.addcrtshow = false;
            $scope.piall1 = pial;
            var toggleStatus = $scope.piall1;
            angular.forEach($scope.indent_list, function (ss) {
                ss.checkedvalue = toggleStatus;
                
            });

            angular.forEach($scope.indent_list, function (mm) {
                if (mm.checkedvalue === true) {
                    $scope.addcrtshow = true;
                }
            });


          
        };
        $scope.optionToggled = function (pial) {
            // $scope.printbill = false;
        };
        $scope.optionToggled1 = function () {
            $scope.addcrtshow = false;
            $scope.all1 = $scope.indent_list.every(function (options) {

                return options.checkedvalue;
            });


            angular.forEach($scope.indent_list, function (mm) {
                if (mm.checkedvalue === true) {
                    $scope.addcrtshow = true;
                }
            });
        }

        $scope.genrateReceipt = function (chk_box) {
            $scope.checkArray = [];
            angular.forEach($scope.get_purchaseindent, function (pi) {
                if (pi.checkedvalue === true) {
                    $scope.checkArray.push(pi);
                }
                
            });
            if ($scope.checkArray.length > 0) {
                $scope.printbill = true;
            }
            else {
                swal("Select any checkbox for generate Indent...!!");
            }

            var data = {
                "checkArray": $scope.checkArray
            };
            apiService.create("OrderApproval/getpidetails", data).
                then(function (promise) {
                    $scope.get_printreceipt = [];
                    $scope.get_PIReceipt = promise.get_PIReceipt;
                    angular.forEach($scope.checkArray, function (pi) {
                        $scope.pi_Receipt = [];
                        var totqty = 0;
                        var totamt = 0;

                        angular.forEach($scope.get_PIReceipt, function (pick) {
                            if (pi.invmpI_Id === pick.INVMPI_Id) {
                                $scope.pi_Receipt.push(pick);
                                totqty += pick.INVTPI_PIQty;
                                totamt += pick.INVTPI_ApproxAmount;
                            }

                        });

                        $scope.get_printreceipt.push({ id: pi.invmpI_Id, result: $scope.pi_Receipt, pinumbr: $scope.pi_Receipt[0].INVMPI_PINo, pidate: $scope.pi_Receipt[0].INVMPI_PIDate, tqty: totqty, tamt: totamt });
                    });

                });
        };

        $scope.printReceipt = function () {
            var innerContents = document.getElementById("printPIReceipt").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
            $scope.printbill = false;
        };


        $scope.Rejectorder = function () {
            var dystring = "";
          
            dystring = "Reject a Purchase Order";
            angular.forEach($scope.pidetails, function (li) {
                if (li.flag === 'A') {
                    approvecnt += 1;
                }
            });
         var   data = {
                "INVTPO_Remarks": $scope.INVTPO_Remarks,
                "INVMPO_TotAmount": $scope.INVMPO_TotAmount,
                "INVMPO_TotRate": $scope.invmpO_TotRate,
                "INVMPO_TotTax": $scope.invmpO_TotTax,
             "arrayPI": $scope.pidetails,
                "INVMPO_Id": $scope.INVMPO_Id,
                "MI_Id": $scope.MI_Id,
                "approvecnt": approvecnt,
            };
               

            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " ???",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + " Reject " + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("OrderApproval/Rejectorder", data).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Purchase Order Rejected Successfully!!!");
                                }
                                else {
                                    swal("Fail to reject the Purchase Order!!!");
                                }

                                $state.reload();
                            });
                    }
                    else {
                        swal("Purchase Order rejection Cancelled!!!");
                    }
                });
        };



      

        $scope.deactiveM = function (item, SweetAlert) {
            $scope.invmpI_Id = item.invmpI_Id;
            var dystring = "";
            if (item.invmpI_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invmpI_ActiveFlg === false) {
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
                        apiService.create("OrderApproval/deactiveM", item).
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
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        };

        $scope.deactive = function (item, SweetAlert) {
            $scope.invtpI_Id = item.invtpI_Id;
            var dystring = "";
            if (item.invtpI_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invtpI_ActiveFlg === false) {
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
                        apiService.create("OrderApproval/deactive", item).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $scope.onformgrid(item);
                                $scope.loaddata();
                            });
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        };


        $scope.radiochange = function (ee) {

            if (ee.flag==='A') {
                $scope.INVMPO_TotAmount = parseFloat($scope.INVMPO_TotAmount) + parseFloat(ee.INVTPO_Amount);
                $scope.invmpO_TotRate = parseFloat($scope.invmpO_TotRate) + parseFloat(ee.INVTPO_RatePerUnit);
                $scope.invmpO_TotTax = parseFloat($scope.invmpO_TotTax) + parseFloat(ee.INVTPO_TaxAmount);
                ee.classname = 'neww';

            }
            else {
                $scope.INVMPO_TotAmount = parseFloat($scope.INVMPO_TotAmount) - parseFloat(ee.INVTPO_Amount);
                $scope.invmpO_TotRate = parseFloat($scope.invmpO_TotRate) - parseFloat(ee.INVTPO_RatePerUnit);
                $scope.invmpO_TotTax = parseFloat($scope.invmpO_TotTax) - parseFloat(ee.INVTPO_TaxAmount);
                ee.classname = 'oldd';
            }

        }

        $scope.columnlist = [];

        $scope.columnlist.push({ ID: 1, coulmn: 'ITEMNAME', title: 'ITEM NAME', checkedvalue: true });
        //$scope.columnlist.push({ ID: 7, coulmn: 'REMARKS', title: 'REMARKS', checkedvalue: true });
        $scope.columnlist.push({ ID: 2, coulmn: 'QUANTITY', title: 'QUANTITY', checkedvalue: true });
        $scope.columnlist.push({ ID: 3, coulmn: 'PRICE/UNIT', title: 'PRICE/UNIT', checkedvalue: true });
        $scope.columnlist.push({ ID: 4, coulmn: 'AMOUNT', title: 'AMOUNT', checkedvalue: true });
        $scope.columnlist.push({ ID: 5, coulmn: 'TAX', title: 'TAX', checkedvalue: true });
        $scope.columnlist.push({ ID: 6, coulmn: 'TOTALAMOUNT', title: 'TOTAL AMOUNT', checkedvalue: true });
        //$scope.viewsavedteple = function (objj) {         
        //    var data = {
        //        "MI_Id": objj.MI_Id,
        //        "INVMS_Id": objj.INVMS_Id,
        //        "INVMPO_Id": objj.INVMPO_Id,
        //        "columnids": $scope.columnlist,
        //        "INVMPO_POTemplate": objj.INVMPO_POTemplate,
        //    }
        //    apiService.create("OrderApproval/viewsavedteple", data).
        //        then(function (promise) {
        //            if (promise.mailmsg !== null) {
        //                $scope.htmldata = promise.mailmsg;
        //                var e1 = angular.element(document.getElementById("test"));
        //                $compile(e1.html($scope.htmldata))(($scope));
        //                $("#templemodel").modal('show');
        //            }
        //        });

        //}
        $scope.printoreder = function () {
            var innerContents = "";
            innerContents = document.getElementById("ordertemplate").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.viewsavedteple = function (user) {
            $scope.invmpO_ReferenceNo = "";
            $scope.pidetailstwo = [];
            $scope.invmpO_IdNew = "";
            $scope.emailid = "";
            $scope.invmpO_IdNew = user.INVMPO_Id;

            document.getElementById("ordertemplate").innerHTML = user.INVMPO_POTemplate;
            $("#popup11134").modal('show');
            angular.forEach($scope.get_supplier, function (dd) {
                if (dd.invmS_Id == user.invmS_Id) {
                    $scope.emailid = dd.invmS_EmailId;
                    return;
                }
            });
        }
        $scope.SendDataSave = function () {
            var msg = document.getElementById("ordertemplate").innerHTML;;
            var data = {
                "msg": msg,
                "ISMMPR_Id": $scope.invmpO_IdNew,
                "ISMINC_WorkOrder": "SAVE"
            };
            apiService.create("ClientInvoice/getreport", data).then(function (promise) {
                swal("Template Save SucessFully ");
                $scope.esubjec = "";
                $scope.MSG = "";
                $scope.FHEAD = "";
                $scope.Footer = "";
                $scope.cfilepath = "";
                $scope.clemail = "";
                $scope.invmpO_IdNew = "";
                $('#popup11134').modal('hide');
                //$scope.pidetails = [];
               // $scope.loaddata();
            });
        }
        $scope.viewtemplate = function (user) {
            $scope.invmpO_ReferenceNo = "";
            $scope.pidetailstwo = [];
            $scope.invmpO_IdNew = "";
            $scope.emailid = "";
            $scope.invmpO_IdNew = $scope.INVMPO_Id;

            document.getElementById("ordertemplate").innerHTML = $scope.template11;
            $("#popup11134").modal('show');
            angular.forEach($scope.get_supplier, function (dd) {
                if (dd.invmS_Id == user.invmS_Id) {
                    $scope.emailid = dd.invmS_EmailId;
                    return;
                }
            });
        }
        $scope.columdetails = [];
        //$scope.viewtemplate = function () {                     
        //    var data = {
        //        "MI_Id": $scope.MI_Id,
        //        "arrayPI": $scope.pidetails,
        //        "INVMPO_Id": $scope.INVMPO_Id,
        //        "columnids": $scope.columnlist,
        //        "INVMPO_POTemplate": $scope.template11,
        //    }
        //    apiService.create("OrderApproval/viewtemplate", data).
        //        then(function (promise) {
        //            if (promise.mailmsg !== null) {
        //                $scope.htmldata = promise.mailmsg;

        //                var e1 = angular.element(document.getElementById("test"));
        //                $compile(e1.html($scope.htmldata))(($scope));
        //                $("#templemodel").modal('show');
        //            }
        //        });
        //}
            
        $scope.edit = function (editid) {
            $scope.template11 = "";
            $scope.MI_Id = editid.MI_Id;
            $scope.cname = editid.MI_Name;
            $scope.inddate = new Date(editid.INVMPO_PODate);
            $scope.refno = editid.INVMPO_ReferenceNo;
            $scope.remarks = editid.INVMPO_Remarks;
            $scope.amount = editid.INVMPO_TotAmount;
            $scope.ttrate = editid.INVMPO_TotRate;
            $scope.tttax = editid.INVMPO_TotTax;
            $scope.piino = editid.INVMPO_PONo;
            $scope.supname = editid.INVMS_SupplierName;
            $scope.supcode = editid.INVMS_SupplierCode;
            $scope.supcpp = editid.INVMS_SupplierConatctPerson;
            $scope.mob = editid.INVMS_SupplierConatctNo;
            $scope.add = editid.INVMS_SupplierAddress;
            $scope.email = editid.INVMS_EmailId;
            $scope.template11 = editid.INVMPO_POTemplate;

            $scope.pidetails = [];
            $scope.INVMPO_TotAmount = editid.INVMPO_TotAmount;
            $scope.invmpO_TotRate = editid.INVMPO_TotRate;
            $scope.invmpO_TotTax = editid.INVMPO_TotTax;
            $scope.prflag = false;
            $scope.editS = true;
            var pi_id = editid.INVMPO_Id;
            $scope.INVMPO_Id = editid.INVMPO_Id;
            var data = {
                "INVMPO_Id": pi_id

            };
            apiService.create("OrderApproval/getpidetails", data).
                then(function (promise) {
                    $scope.edit_pimodel = promise.get_pimodel;
                    angular.forEach($scope.edit_pimodel, function (editli) {
                        var flag = '';
                        var classname='';
                        if (editli.INVTPOAPP_RejectFlg===true) {
                            flag = 'R';
                            classname = 'oldd';

                        } else {
                            flag = 'A';
                            classname = 'neww';
                        }

                        $scope.pidetails.push({
                            INVMPO_Id: editli.INVMPO_Id, INVMI_Id: editli.INVMI_Id, invmI_ItemName: editli.INVMI_ItemName, invmuoM_Id: editli.INVMUOM_Id,
                            invmuoM_UOMName: editli.INVMUOM_UOMName, INVTPO_POQty: editli.INVTPO_POQty,
                            INVTPO_RatePerUnit: editli.INVTPO_RatePerUnit, INVTPO_Amount: editli.INVTPO_Amount, INVTPO_TaxAmount: editli.INVTPO_TaxAmount,
                            INVTPO_Remarks: editli.INVTPO_Remarks, flag: flag, INVTPO_ApproveQty: editli.INVTPO_ApproveQty, INVTPO_Id: editli.INVTPO_Id, INVMPI_Id: editli.INVMPI_Id, INVMPO_POTemplate: editli.INVMPO_POTemplate, classname: classname
                        });
                    });


                    $scope.viewcomments = [];
                    if (promise.viewcomments !== undefined && promise.viewcomments !== null && promise.viewcomments.length > 0) {
                        $scope.viewcomments = promise.viewcomments;
                    }

                });
        };

        $scope.OnClickViewComments = function () {
            $('#modalViewComments').modal('show');
        };

        $scope.OnClickIndividualViewComments = function (dd) {

            var data = {
                "INVTPO_Id": dd.INVTPO_Id,
                "INVMPO_Id": dd.INVMPO_Id
            };

            apiService.create("OrderApproval/OnClickIndividualViewComments", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.indiviewcomments !== undefined && promise.indiviewcomments !== null && promise.indiviewcomments.length > 0) {
                        $scope.indiviewcomments = promise.indiviewcomments;
                        $('#modalindiViewComments').modal('show');
                    }
                }
            });

        };



        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';


        $scope.onformdetails = function (itemid) {
            $scope.get_prDetail = "";

            var item_id = itemid.invmpR_Id;
            var data = {
                "INVMPR_Id": item_id
            };
            apiService.create("INV_PurchaseRequisition/get_prdetails", data).
                then(function (promise) {
                    $scope.get_prDetail = promise.get_prDetail;
                    $scope.prnum = $scope.get_prDetail[0].invmpR_PRNo;
                    $scope.reqname = $scope.get_prDetail[0].employeename;
                });
        };

        $scope.onformgrid = function (itemid) {
            $scope.get_pimodel = "";
            var item_id = itemid.invmpI_Id;
            var data = {
                "INVMPI_Id": item_id
            };
            apiService.create("OrderApproval/getpidetails", data).
                then(function (promise) {
                    $scope.get_pimodel = promise.get_pimodel;
                    $scope.pinum = $scope.get_pimodel[0].invmpI_PINo;
                });
        };

    }
})();