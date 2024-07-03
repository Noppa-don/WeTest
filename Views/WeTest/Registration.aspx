<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/ms2018.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<link href="/content/Wetest/Wetest.css?ver=1.1.1" rel="stylesheet" type="text/css" />
    <link href="/content/Wetest/Registration.css?ver=1.1.1" rel="stylesheet" type="text/css" />
    <script src="/scripts/Wetest/Registration.js?ver=1.1.1" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="banner">
        <div class='logo'></div>
        <div class='pagename'>- Register</div>
    </div>

    <div class='wrapper'>
        <div class='register ui-hide'>
            <div class="registerData">
                <div class="registerDetail">
                    <div class="userType">
                        <div class="btn left" id="btnStudent">Student / School</div>
                        <div class="btn right" id="btnOther">Officer / Other</div>
                    </div>
                    <br />
                    <br />
                    <div>
                        <span class="captionName ui-hide" id="capType">Type</span>
                        <div class="spnData"><span class="spType"></span></div>
                    </div>
                    <div>
                        <span class="captionName">First name</span>
                        <div class="txtData"><input type='text' id='Firstname' title='Firstname' placeholder='First name' /></div>
                        <div class="spnData"><span class="spFirstname"></span></div>
                    </div>
                    <div>
                        <span class="captionName">Surname</span>
                        <div class="txtData">
                            <input type='text' id='Surname' title='Surname' placeholder='Surname' /></div>
                        <div class="spnData"><span class="spSurname"></span></div>
                    </div>
                    <div>
                        <span class="captionName">Mobile No.</span>
                        <div class="txtData">
                            <input type="text" maxlength="10" id='MobileNo' title='MobileNo' placeholder='xxx-xxx-xxxx' /></div>
                        <div class="spnData"><span class="spMobileNo"></span></div>
                    </div>
                    <div>
                        <span class="captionName">E-Mail</span>
                        <div class="txtData">
                            <input type='text' id='EMail' title='EMail' placeholder='E-Mail' /></div>
                        <div class="spnData"><span class="spEMail"></span></div>
                    </div>
                    <div>
                        <span class="captionName">Username</span>
                        <div class="txtData">
                            <input type='text' id='Username' title='Username' placeholder='Username' /></div>
                        <div class="spnData"><span class="spUsername"></span></div>
                    </div>
                    <div>
                        <span class="captionName" id="captionPassword">Password</span>
                        <div class="txtData">
                            <input type="password" id='Password' title='Password' placeholder='Password' /></div>
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
                    <input type="file" id="file" class="ui-hide"/>
                    <div class='Imagebtn btnPhoto'></div>
                </div>
             
            </div>
        </div>
        <div class="otp">
            <span>Send OTP to your mobile now,<br />Please type OTP and click confirm.</span>
            <br /><br />
            <input type='text' id='txtOTP' title='OTP' placeholder='OTP' />
            <br /><br />
            <span>If you not recive OTP. Please click send again.</span>
               <br /> <br />   <br /> <br />
             <div class="btnOTP">
                <div class="btn btnUnActive ui-Left" id="btnSendAgain">Send again</div>
                <div class="btn ui-Right" id="btnConfirm">Confirm</div>
            </div>
        </div>
        <div class="payment">

		</div>
        <div class="refillKey"></div>
    </div>

    <div class="footer">
         <div class='footerRegister'>
            <div class="Imagebtn btnBack" id="btnBack"></div>
            <div class="Imagebtn btnNext" id="btnNext"></div>
         </div>
    </div>
</asp:Content>
