<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/ms2018.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../content/Wetest/weTestCashier.css?ver=1.1.6" rel="stylesheet" type="text/css" />
    <link href="../content/Wetest/Admin.css?ver=1.1.6" rel="stylesheet" type="text/css" />
    <script src="../scripts/Wetest/Admin.js?ver=1.1.6" type="text/javascript"></script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="banner">
        <div class="AppData">
            <div class="logo"></div>
            <div class='pagename'></div>
        </div>
        <div class="UserData flexDiv ui-hide">
            <div class="UserNameandLevel"></div>
        </div>
        <div class="UserMenu ui-hide">
            <div class="btnAccountMenu EditAccount flexDiv">
                <div class="Accountlogo firstflexdiv"></div>
                <div>Account</div>
            </div>
            <div class="btnAccountMenu Setting flexDiv">
                <div class="Accountlogo firstflexdiv"></div>
                <div>Setting</div>
            </div>
            <div class="btnAccountMenu Logout flexDiv">
                <div class="Logoutlogo firstflexdiv"></div>
                <div class="firstflexdiv">Log out</div>
            </div>
            <div class="btnAccountMenu DeleteAccount flexDiv">
                <div class="Deletelogo firstflexdiv"></div>
                <div>Delete Account</div>
            </div>
        </div>
    </div>

    <div class='wrapper'>
        <div class="login ui-hide">
            <div>
                <div class="smallLogo userlogo"></div>
                <input type='text' id='userName' title='Username' placeholder='Username' />
            </div>
            <br />
            <div class='last'>
                <div class="smallLogo pswlogo"></div>
                <input type='password' id='userPass' title='Password' placeholder='Password' />
            </div>
            <div id="btnLogin" class='btn btnSelected'>Log-in</div>

            <div class='textRight registerlink'>Register ?</div>
        </div>
        <div class="MainMenu">
            <div class="menuButton flexDiv">
                <div class="btn btnMainMenu" id="btnPaymentList">
                    <div class='paymentListIcon'></div>
                    <span class="txtMenu">Payment list</span>
                </div>
                <div class="btn btnMainMenu" id="btnReport">
                    <div class='reportIcon'></div>
                    <span class="txtMenu">Reports</span>
                </div>
            </div>
            <div class="expiredDate"></div>
        </div>
        <div class="Setting ui-hide">
            Notification
        </div>

    </div>
    <div class="PaymentList flexDiv ui-hide">
        <div class="JobMenu firstflexdiv">
            <div class="jobdiv Active" id="doingJob" jval="1">งานที่ต้องทำ</div>
            <div class="jobdiv" id="problemPayment" jval="2">การชำระเงินที่มีปัญหา</div>
            <div class="jobdiv" id="successPayment" jval="3">ชำระเงินสำเร็จ</div>
        </div>
        <div class="JobDetailContainer">
            <div class="FilterJob flexDiv">

                <div class="filterdiv Active firstflexdiv" id="simplePackage" fval="1">แพกเกจปกติ</div>
                <div class="filterdiv" id="discountPackage" fval="2">ส่วนลด</div>
                <div class="filterdiv" id="keycodePackage" fval="3">ทดลองใช้ / ใช้ฟรี</div>
            </div>
            <div class="JobDetail"></div>
        </div>
    </div>
    <div class="PaymentDetail flexdiv ui-hide">
        <div class="firstflexdiv">
            <div class="slipDetail"></div>
            <br />
            <div class="divUploadSlip">
                <span>Add bank tranfer image here</span>
                <br />
                <div class="flexDiv">
                    <input type='text' id='SlipName' />
                    <input type='file' id='fileSlip' class="ui-hide notborder" />
                    <div class='Imagebtn btnSlipPhoto'></div>
                </div>
            </div>
        </div>
        <div class="slipPhoto"></div>

        <div class="Imagebtn btnCancelConfirmSlip"></div>
    </div>
    <div class="report flexDiv ui-hide">
        <div class="Filter firstflexdiv">
            
            <div id="TimeData" class="flexDiv">
                <div>
                    <label for="rdbThisWeek" class="rdb">This week</label><input type="radio" name="radio-1" class="firstflexdiv" id="rdbThisWeek" />
                </div>
                <div>
                    <label for="rdbMonth" class="rdb">This month</label><input type="radio" name="radio-1" id="rdbMonth" />
                </div>
                <div>
                    <label for="rdbChooseDate" class="rdb">Choose from date</label><input type="radio" name="radio-1" id="rdbChooseDate" />
                </div>
                <div>
                    <input type='text' class="filterdate ui-hide" id='StartDate' />
                </div>
                <div class="calendarlogo unActive" id="btnStartDate"></div>

                <div>
                    <input type='text' class="filterdate ui-hide" id='EndDate' />
                </div>
                <div class="calendarlogo unActive" id="btnEndDate"></div>
            </div>
        </div>
        <div>
            <div class="Imagebtn unActive" id="btnSearch"></div>
        </div>
    </div>
    <div class="footer">
        <div class='footerButton footerslip ui-hide'>
            <div class="Imagebtn btnReject fistflexdiv"></div>
            <div class="Imagebtn btnConfirmSlip ui-Right"></div>
        </div>
        <div class='footerButton footerUpdateRejectSlip ui-hide'>
            <div class="Imagebtn btnReject2 fistflexdiv"></div>
            <div class="Imagebtn btnUploadNewSlip ui-Right"></div>
        </div>
    </div>

    <div name='select' id="dialogConfirm" class='my-popup confirm ui-popup-container ui-popup-hidden'>
        <div class='ui-content ui-body-c ui-corner-all ui-shadow'>
            <div class='ui-icon warning'></div>
            <div class='ui-text'>Confirm this payment?</div>
            <div class="ui-twoButton">
                <div class='btn btnCancel btnNo'>No</div>
                <div class='btn btnSelected btnconfirm'>Confirm</div>
            </div>
        </div>
    </div>
    <div name='select' id="dialogReject" class='my-popup confirm ui-popup-container ui-popup-hidden'>
        <div class='ui-content ui-body-c ui-corner-all ui-shadow'>
            <div class='ui-icon warning'></div>
            <div class='ui-text'>Reject this payment?</div>
            <div class="txtData">
                Reason
                <input type='text' id='txtReason' />
            </div>
            <br />
            <div class="ui-twoButton">
                <div class='btn btnCancel btnNo'>No</div>
                <div class='btn btnSelected btnRejectConfirm'>Confirm</div>
            </div>
        </div>
    </div>

</asp:Content>
