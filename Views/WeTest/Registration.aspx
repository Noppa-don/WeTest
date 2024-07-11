<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/ms2018.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../content/Wetest/Wetest.css?ver=1.1.2" rel="stylesheet" type="text/css" />
    <link href="../content/Wetest/Registration.css?ver=1.1.2" rel="stylesheet" type="text/css" />
    <script src="../scripts/Wetest/Registration.js?ver=1.1.2" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="banner">
        <div class='logo'></div>
        <div class='pagename'>- Register</div>
    </div>

    <div class='wrapper'>

        <div class='register'>
            <div class="registerData">
                <div class="registerDetail">
                    <div class="userType">
                        <div class="btn left" id="btnStudent" stdType="1">Student / School</div>
                        <div class="btn right" id="btnOther" stdType="2">Officer / Other</div>
                    </div>
                    <br />
                    <br />
                    <div>
                        <span class="captionName ui-hide" id="capType">Type</span>
                        <div class="spnData"><span class="spType"></span></div>
                    </div>
                    <div>
                        <span class="captionName">First name</span>
                        <div class="txtData">
                            <input type='text' id='Firstname' title='Firstname' placeholder='First name' />
                        </div>
                        <div class="spnData"><span class="spFirstname"></span></div>
                    </div>
                    <div>
                        <span class="captionName">Surname</span>
                        <div class="txtData">
                            <input type='text' id='Surname' title='Surname' placeholder='Surname' />
                        </div>
                        <div class="spnData"><span class="spSurname"></span></div>
                    </div>
                    <div>
                        <span class="captionName">Mobile No.</span>
                        <div class="txtData">
                            <input type="text" maxlength="10" id='MobileNo' title='MobileNo' placeholder='xxx-xxx-xxxx' />
                        </div>
                        <div class="spnData"><span class="spMobileNo"></span></div>
                    </div>
                    <div>
                        <span class="captionName">E-Mail</span>
                        <div class="txtData">
                            <input type='text' id='EMail' title='EMail' placeholder='E-Mail' />
                        </div>
                        <div class="spnData"><span class="spEMail"></span></div>
                    </div>
                    <div>
                        <span class="captionName">Username</span>
                        <div class="txtData">
                            <input type='text' id='Username' title='Username' placeholder='Username' />
                        </div>
                        <div class="spnData"><span class="spUsername"></span></div>
                    </div>
                    <div>
                        <span class="captionName" id="captionPassword">Password</span>
                        <div class="txtData">
                            <input type="password" id='Password' title='Password' placeholder='Password' />
                        </div>
                    </div>
                    <div>
                        <span class="captionName" id="captionConfirmPassword">Confirm Password</span>
                        <div class="txtData">
                            <input type="password" id='ConfirmPassword' title='ConfirmPassword' placeholder='Confirm Password' />
                        </div>
                    </div>
                </div>
                <div class='divPhoto'>
                    <br />
                    <span class="captionPhoto">Your Photo</span>
                    <br />
                    <br />
                    <input type="file" id="file" style="display: none;" />
                    <div class='Imagebtn btnPhoto'></div>
                </div>
            </div>
        </div>

        <div class="otp ui-hide">
            <span>Send OTP to your mobile now,<br />
                <br />
                Please type OTP and click confirm.</span>
            <br />
            <br />
            <input type='text' id='txtOTP' title='OTP' placeholder='OTP' />
            <br />
            <br />
            <span>If you not recive OTP. Please click send again.</span>
            <br />
            <br />
            <br />
            <br />
            <div class="btnOTP">
                <div class="btn btnUnActive ui-Left" id="btnSendAgain">Send again</div>
                <div class="btn ui-Right" id="btnConfirm">Confirm</div>
            </div>
        </div>

        <div class="payment ui-hide">
            <span>You must have a Wetest Key for register<br />
                <br />
                ** Price per Account is <span id="PackagePrice"></span> Bath / 1 Year **<br />
            </span>
            <br />
            <span class="ui-Warning-green ui-hide" id="spnWarningDiscount">Promotion code / Discount code is already !<br /><br /></span>
            <span class="ui-Warning-red" id="spnWarning">Please press "Payment" for pay<br />You can use Mobile Banking for this.</span>
            <br />
            <br />
            <div class="btn" id="btnPayment">Payment</div>
            <div class="Imagebtn btnQR ui-hide"></div>
            <br />
            <br />
            <span>or if you have a key. Please type.</span>
            <br />
            <br />
            <div class="divKeyCode">
                <input type='text' id='txtKeyCode' placeholder='Type Key' />
            </div>
            <br />
            <div class="btn" id="btnCheckKey">Check Key</div>
            <br />
            <span class="warningText">If you have a question and promblem.<br />
                Please contact to @Italt</span>
        </div>

    </div>

    <div class="footer">
        <div class='footerButton footerRegister'>
            <div class="Imagebtn btnBack"></div>
            <div class="Imagebtn btnNext"></div>
        </div>
        <div class='footerButton footerOTP ui-hide'>
            <div class="Imagebtn btnBack"></div>
            <div class="Imagebtn btnNext"></div>
        </div>
        <div class='footerButton footerPayment  ui-hide'>
            <div class="Imagebtn btnBack"></div>
            <div class="btn btnSelected" id="btnDiscount">Have discount?</div>
        </div>
    </div>

    <div name='alert' id="dialogAlert" class='my-popup alert ui-popup-container ui-popup-hidden'>
        <div class='ui-content ui-body-c ui-corner-all ui-shadow'>
            <div class='ui-icon wrong'></div>
            <div class='ui-text'></div>
        </div>
    </div>

    <div name='alert' id="dialogGotoQuiz" class='my-popup alert ui-popup-container ui-popup-hidden'>
        <div class='ui-content ui-body-c ui-corner-all ui-shadow'>
            <div class='ui-icon wait'></div>
            <div class='ui-text'>Please wait...We are taking you to Placement Test.</div>
        </div>
    </div>

    <div name='confirm' id="dialogConfirm" class='my-popup alert ui-popup-container ui-popup-hidden'>
        <div class='ui-content ui-body-c ui-corner-all ui-shadow'>
            <div class='ui-icon wrong'></div>
            <div class='ui-text'></div>
            <div id="btnOK" class='btn btnSelected'>OK</div>
        </div>
    </div>

    <div name='select' id="dialogSelect" class='my-popup confirm ui-popup-container ui-popup-hidden'>
        <div class='ui-content ui-body-c ui-corner-all ui-shadow'>
            <div class='ui-icon done'></div>
            <div class='ui-text'></div>
            <div class="ui-twoButton">
                <div class='btn btnCancel'>Later</div>
                <div class='btn btnSelected'>Sure!</div>
            </div>
        </div>
    </div>

    <div name='discount' id="dialogDiscount" class='my-popup confirm ui-popup-container ui-popup-hidden'>
        <div class='ui-icon close'></div>
        <div class='ui-content ui-body-c ui-corner-all ui-shadow'>
            <div class='ui-textHeader'>Discount for Wetest</div><br>
            <div class='ui-text'>Type Promotion code / Discount code</div>
             <div class="divKeyCode"><input type='text' id='txtDiscountCode' placeholder='Promotion code / Discount code' /></div><br>
            <div class='ui-Warning-red ui-hide'></div>
            <div id="btnCheckDiscountCode" class='btn btnSelected'>Apply Code</div>
        </div>
    </div>

</asp:Content>
