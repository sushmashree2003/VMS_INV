
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ProformaInvoiceController', ProformaInvoiceController);
    ProformaInvoiceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function ProformaInvoiceController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.obj = {};
        $scope.editS = false;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        $scope.get_checkout = [];
        //===================================== VIEW TEMPLATE
        //====================== Auto Generated Number
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                if (transnumconfigsettings[i].imN_Flag === "ProformaInvoice") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }
        //===========================

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //-------------------------------------------------------------------
        $scope.obj = {};
        $scope.invacO_CheckoutDate = new Date();
        $scope.currentPage = 1;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.itemsPerPage1 = paginationformasters;
        $scope.tempdc = [];
        $scope.loaddata = function () {

            $scope.search = "";
            var pageid = 2;
            apiService.getURI("ProformaInvoice/getloaddata", pageid).
                then(function (promise) {
                   
                 
                    $scope.instlist = promise.instlist;

                    $scope.get_Sale = promise.get_Sale;
                 

                    $scope.imagename = $scope.instlist[0].mI_Logo;
                    $scope.miname = $scope.instlist[0].mI_Name;
                    

                });
        };

        ///

        $scope.printInvoice = function () {

            $('#popup111').modal('show');


        };
        //============================================ 
        $scope.reschedule = function (obj) {

            $scope.ttl = 0;
            $scope.imagename = $scope.instlist[0].mI_Logo;
            $scope.miname = $scope.instlist[0].mI_Name;
            //$('#popup111').modal('show');

            $scope.printstudents = [];
            angular.forEach(obj.LIST, function (dd) {
                    $scope.ttl = $scope.ttl + dd.invacO_CheckOutQty;
                    $scope.printstudents.push(dd);
                $scope.dcdate = dd.updatedDate;

            })

            $scope.tempto = [];
            angular.forEach($scope.printstudents, function (ddd) {

                if ($scope.tempto.length == 0) {
                    $scope.tempto.push({ invmlO_Id: ddd.invmlO_Id, invmlO_LocationRoomName: ddd.invmlO_LocationRoomName });
                }
                else if ($scope.tempto.length > 0) {
                    var already_cnt = 0;
                    angular.forEach($scope.tempto, function (opq1) {
                        if (opq1.invmlO_Id == ddd.invmlO_Id) {
                            already_cnt += 1;
                        }
                    })

                    if (already_cnt == 0) {
                        $scope.tempto.push({ invmlO_Id: ddd.invmlO_Id, invmlO_LocationRoomName: ddd.invmlO_LocationRoomName });
                    }

                }
            })

            $scope.temptorecby = [];
            $scope.temptorecbyname = '';
            angular.forEach($scope.printstudents, function (ddd) {

                if ($scope.temptorecby.length == 0) {
                    $scope.temptorecby.push({ invacO_ReceivedBy: ddd.invacO_ReceivedBy });
                    $scope.temptorecbyname = ddd.invacO_ReceivedBy;
                }
                else if ($scope.temptorecby.length > 0) {
                    var already_cnt = 0;
                    angular.forEach($scope.temptorecby, function (opq1) {
                        if (opq1.invacO_ReceivedBy == ddd.invacO_ReceivedBy) {
                            already_cnt += 1;
                        }
                    })

                    if (already_cnt == 0) {
                        $scope.temptorecby.push({ invacO_ReceivedBy: ddd.invacO_ReceivedBy });
                        $scope.temptorecbyname = $scope.temptorecbyname +' , ' + ddd.invacO_ReceivedBy;
                    }

                }
            })



            $scope.decno = obj.INVACO_No;
           
           
            $('#popup111').modal('show');

        }


        //===================== Get Item on store Change
        $scope.onstorechange = function () {
            $scope.id = $scope.obj.invmsT_Id.invmsT_Id;
            var data = {
                "INVMST_Id": $scope.id
            };
            apiService.create("INV_T_Sales/getitem", data).
                then(function (promise) {

                    if (promise.get_item.length > 0) {
                        $scope.get_items = promise.get_item;
                    }
                    else {
                        swal('For Selected Store, No Item found..!!');
                        $scope.get_items.length = 0;
                    }
                });
        };
        //$scope.onstorechange = function () {
        //    $scope.id = $scope.obj.invmsT_Id.invmsT_Id;
        //    var data = {
        //        "INVMST_Id": $scope.id
        //    };
        //    apiService.create("ProformaInvoice/getitems", data).
        //        then(function (promise) {
        //            $scope.get_items = promise.get_items;
        //        });
        //};
        $scope.onlocationchange = function () {
            $scope.id = $scope.obj.invmlO_Id.invmlO_Id;
            var data = {
                "INVMLO_Id": $scope.id
            };
            apiService.create("ProformaInvoice/getcontactperson", data).
                then(function (promise) {
                    $scope.get_contactperson = promise.get_contactperson;
                    $scope.contactflag = promise.contactflag;
                    if ($scope.contactflag === "E") {
                        $scope.hrmE_Id = $scope.get_contactperson[0].hrmE_Id;
                        $scope.employeename = $scope.get_contactperson[0].employeename;
                    }
                    else {
                        $scope.get_employee = promise.get_employee;
                        $scope.employeename = $scope.get_contactperson[0].invmlO_InchargeName;
                    }
                });
        };
        $scope.onitemchange = function () {
            var data = {};
            var avstock = 0;
            $scope.storeid = $scope.obj.invmsT_Id.invmsT_Id;
            $scope.itemid = $scope.obj.INVMI_Id.INVMI_Id;
            $scope.salerate = $scope.obj.INVMI_Id.INVSTO_SalesRate;
            data = {
                "INVMI_Id": $scope.itemid,
                "INVSTO_SalesRate": $scope.salerate,
                "INVMST_Id": $scope.storeid
            };
            apiService.create("INV_T_Sales/getitemDetail", data).
                then(function (promise) {
                    $scope.availablestock = promise.availablestock;
                    angular.forEach($scope.availablestock, function (ast) {
                        avstock += ast.AvaiableStock;
                    });
                    $scope.avstock = avstock;
                });
        };
        $scope.printstudents = [];
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.get_checkout, function (itm) {
                itm.selected = toggleStatus;
               
            });
        }


        $scope.optionToggled1 = function (SelectedStudentRecord, index) {

            $scope.all = $scope.get_checkout.every(function (options) {

                return options.selected;
            });
        }


        $scope.checkStock = function () {
            var totlqty = 0;
            var availablestok = 0;
            totlqty = parseInt($scope.invacO_CheckOutQty);
            availablestok = parseInt($scope.avstock);
            if (totlqty > availablestok) {
                swal("Please Check Available Stock...!!");
                $scope.invacO_CheckOutQty = "";
            }
        };


        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("asdf").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        $scope.ttl = 0;
        $scope.generatedc = function () {
            debugger;
            $scope.ttl = 0;
            $scope.imagename = $scope.instlist[0].mI_Logo;
            $scope.miname = $scope.instlist[0].mI_Name;
            //$('#popup111').modal('show');

            $scope.printstudents = [];
            angular.forEach($scope.get_checkout, function (dd) {
                if (dd.selected == true) {
                    $scope.ttl = $scope.ttl + dd.invacO_CheckOutQty;
                    $scope.printstudents.push(dd);
                }

            })


            $scope.tempto = [];
            angular.forEach($scope.printstudents, function (ddd) {

                if ($scope.tempto.length == 0) {
                    $scope.tempto.push({ invmlO_Id: ddd.invmlO_Id, invmlO_LocationRoomName: ddd.invmlO_LocationRoomName });
                }
                else if ($scope.tempto.length > 0) {
                    var already_cnt = 0;
                    angular.forEach($scope.tempto, function (opq1) {
                        if (opq1.invmlO_Id == ddd.invmlO_Id) {
                            already_cnt += 1;
                        }
                    })

                    if (already_cnt == 0) {
                        $scope.tempto.push({ invmlO_Id: ddd.invmlO_Id,invmlO_LocationRoomName: ddd.invmlO_LocationRoomName });
                    }

                }
            })


            $scope.temptorecby = [];
            $scope.temptorecbyname = '';
            angular.forEach($scope.printstudents, function (ddd) {

                if ($scope.temptorecby.length == 0) {
                    $scope.temptorecby.push({ invacO_ReceivedBy: ddd.invacO_ReceivedBy });
                    $scope.temptorecbyname = ddd.invacO_ReceivedBy;
                }
                else if ($scope.temptorecby.length > 0) {
                    var already_cnt = 0;
                    angular.forEach($scope.temptorecby, function (opq1) {
                        if (opq1.invacO_ReceivedBy == ddd.invacO_ReceivedBy) {
                            already_cnt += 1;
                        }
                    })

                    if (already_cnt == 0) {
                        $scope.temptorecby.push({ invacO_ReceivedBy: ddd.invacO_ReceivedBy });
                        $scope.temptorecbyname = $scope.temptorecbyname + ' , ' + ddd.invacO_ReceivedBy;
                    }

                }
            })

            if ($scope.printstudents.length == 0) {
                swal('Select Items to generate Delivery Challan');
            }
            else {
                var data = {};

         
                    data = {
                        selecteditems: $scope.printstudents,
                       
                        transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
                    };
             
              

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("ProformaInvoice/savedetails", data).then(function (promise) {

                    if (promise.returnval === true) {

                        $scope.decno = promise.trans_id;
                        $scope.dcdate = promise.updatedDate;
                        $scope.get_checkout = promise.get_checkout;
                        $scope.get_checkoutdclist = promise.get_checkoutdclist;


                        $scope.tempdc = [];
                        angular.forEach($scope.get_checkoutdclist, function (ddd) {

                            if ($scope.tempdc.length == 0) {
                                $scope.tempdc.push({ INVACO_No: ddd.invacO_No });
                            }
                            else if ($scope.tempdc.length > 0) {
                                var already_cnt = 0;
                                angular.forEach($scope.tempdc, function (opq1) {
                                    if (opq1.INVACO_No == ddd.invacO_No) {
                                        already_cnt += 1;
                                    }
                                })

                                if (already_cnt == 0) {
                                    $scope.tempdc.push({ INVACO_No: ddd.invacO_No });
                                }

                            }
                        })

                        $scope.mainarray = [];
                        angular.forEach($scope.tempdc, function (ss) {
                            var listdat = [];
                            angular.forEach($scope.get_checkoutdclist, function (ff) {
                                if (ss.INVACO_No == ff.invacO_No) {
                                    listdat.push(ff);
                                }

                            })
                            ss.LIST = listdat;
                        })
                        $('#popup111').modal('show');

                    }
                    else {
                        swal('Error in DC generation');
                    }

                });
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };
        $scope.griddtl = false;
        $scope.edit = function (item, salerate) {
            $scope.mainttl = 0;
            $scope.get_SaleItemDetails =[];
            $scope.get_SaleItemTax = [];
            $scope.ttl = 0;
            $scope.griddtl = false;
            $scope.editS = true;
            $scope.INVMSL_Id = item.INVMSL_Id;
            $scope.invmS_StoreName = item.INVMS_StoreName;
            $scope.client = item.ISMMCLT_ClientName;
            $scope.INVMSL_SalesValue = item.INVMSL_SalesValue;
         
            $scope.INVMSL_SalesDate = item.INVMSL_SalesDate;
            $scope.INVMSL_Remarks = item.INVMSL_Remarks;
           
            $scope.INVMSL_SalesNo = item.INVMSL_SalesNo;
            $scope.INVMSL_SalesNo = item.INVMSL_SalesNo;
            $scope.INVMSL_SalesDate = item.INVMSL_SalesDate;

            $scope.INVMSL_TotTaxAmt = item.INVMSL_TotTaxAmt;
            $scope.INVMSL_TotDiscount = item.INVMSL_TotDiscount;
            $scope.INVMSL_TotalAmount = item.INVMSL_TotalAmount;
            $scope.INVMSL_Remarks = item.INVMSL_Remarks;
            $scope.ISMMCLT_Address = item.ISMMCLT_Address;
            $scope.ISMMCLT_Desc = item.ISMMCLT_Desc;
            $scope.per = 100;
            $scope.ttln = 0;
            $scope.mainttl = 0;
            var avstock = 0;
            $scope.taxtotal = 0;
            $scope.roundoffttl = 0;
            var data = {
                "INVMSL_Id": $scope.INVMSL_Id
            };
            apiService.create("ProformaInvoice/getitemDetail", data).
                then(function (promise) {
                    debugger;
                    $scope.griddtl = true;
                    $scope.get_SaleItemDetails = promise.get_SaleItemDetails;
                    $scope.get_SaleItemTax = promise.get_SaleItemTax;



                    angular.forEach($scope.get_SaleItemDetails, function (ee) {

                        $scope.ttl = $scope.ttl + ee.invtsL_Amount;
                    });

                    $scope.ttln = ($scope.per / 100) * $scope.ttl;


                    $scope.masterTax = [];
                   
                    angular.forEach($scope.get_SaleItemTax, function (ddd) {
                        if ($scope.masterTax.length == 0) {
                            $scope.masterTax.push({ INVMT_Id: ddd.invmT_Id, INVMT_TaxName: ddd.invmT_TaxName });
                           
                        }
                        else if ($scope.masterTax.length > 0) {
                            var already_cnt = 0;
                            angular.forEach($scope.masterTax, function (opq1) {
                                if (opq1.INVMT_Id == ddd.invmT_Id) {
                                    already_cnt += 1;
                                }
                            })
                            if (already_cnt == 0) {
                                $scope.masterTax.push({ INVMT_Id: ddd.invmT_Id, INVMT_TaxName: ddd.invmT_TaxName });
                            }

                        }
                    })
                    $scope.taxtotal = 0;
                    angular.forEach($scope.masterTax, function (ff) {
                      var  taxttl=0
                        angular.forEach($scope.get_SaleItemTax, function (gg) {
                            if (ff.INVMT_Id == gg.invmT_Id) {
                                taxttl = taxttl + gg.invtslT_TaxAmt;
                                $scope.taxtotal = $scope.taxtotal + gg.invtslT_TaxAmt;
                            }


                        })
                        $scope.mainttl = $scope.mainttl + taxttl;
                        ff.Tl = taxttl;

                    })
                    
                    $scope.mainttl = $scope.mainttl + $scope.ttln;



                    $scope.Math = window.Math;
                    $scope.roundoffttl = $scope.Math.round($scope.mainttl, 0)
                    $scope.words = $scope.amountinwords($scope.roundoffttl);
                   

                });
           
        };

        $scope.changeadvance = function () {
            $scope.ttln = ($scope.per / 100) * $scope.ttl;

            $scope.mainttl = $scope.taxtotal + $scope.ttln;

            

            debugger;
            $scope.Math = window.Math;
            $scope.roundoffttl = $scope.Math.round($scope.mainttl, 0) 
            $scope.words = $scope.amountinwords($scope.roundoffttl);
            console.log($scope.words);
        }

        $scope.deactive = function (item, SweetAlert) {
            $scope.INVACO_Id = item.invacO_Id;
            var dystring = "";
            if (item.invacO_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invacO_ActiveFlg === false) {
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
                        apiService.create("ProformaInvoice/deactive", item).
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

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
        $scope.searchValue1 = '';


        //======Amount Convert into word=========//
        $scope.amountinwords = function convertNumberToWords(atotalc) {
            var words = new Array();
            words[0] = '';
            words[1] = 'One';
            words[2] = 'Two';
            words[3] = 'Three';
            words[4] = 'Four';
            words[5] = 'Five';
            words[6] = 'Six';
            words[7] = 'Seven';
            words[8] = 'Eight';
            words[9] = 'Nine';
            words[10] = 'Ten';
            words[11] = 'Eleven';
            words[12] = 'Twelve';
            words[13] = 'Thirteen';
            words[14] = 'Fourteen';
            words[15] = 'Fifteen';
            words[16] = 'Sixteen';
            words[17] = 'Seventeen';
            words[18] = 'Eighteen';
            words[19] = 'Nineteen';
            words[20] = 'Twenty';
            words[30] = 'Thirty';
            words[40] = 'Forty';
            words[50] = 'Fifty';
            words[60] = 'Sixty';
            words[70] = 'Seventy';
            words[80] = 'Eighty';
            words[90] = 'Ninety';
            atotalc = atotalc.toString();
            var atemp = atotalc.split(".");
            var number = atemp[0].split(",").join("");
            var n_length = number.length;
            var words_string = "";
            if (n_length <= 9) {
                var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);
                var received_n_array = new Array();
                for (var i = 0; i < n_length; i++) {
                    received_n_array[i] = number.substr(i, 1);
                }
                for (var i = 9 - n_length, j = 0; i < 9; i++ , j++) {
                    n_array[i] = received_n_array[j];
                }
                for (var i = 0, j = 1; i < 9; i++ , j++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        if (n_array[i] == 1) {
                            n_array[j] = 10 + parseInt(n_array[j]);
                            n_array[i] = 0;
                        }
                    }
                }
                atotalc = "";
                for (var i = 0; i < 9; i++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        atotalc = n_array[i] * 10;
                    } else {
                        atotalc = n_array[i];
                    }
                    if (atotalc != 0) {
                        words_string += words[atotalc] + " ";
                    }
                    if ((i == 1 && atotalc != 0) || (i == 0 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Crores ";
                    }
                    if ((i == 3 && atotalc != 0) || (i == 2 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Lakhs ";
                    }
                    if ((i == 5 && atotalc != 0) || (i == 4 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Thousand ";
                    }
                    if (i == 6 && atotalc != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
                        words_string += "Hundred and ";
                    } else if (i == 6 && atotalc != 0) {
                        words_string += "Hundred ";
                    }
                }
                words_string = words_string.split("  ").join(" ");
            }
            return words_string;
        }
        //==============End==============//
    }
})();