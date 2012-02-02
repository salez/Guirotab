<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>CadastroPreVisualizar</title>
    <% Html.RenderPartial("Header"); %>

    <script type="text/javascript">

            $(function () {

                setTimeout(function () {
                    $('#pre-visualizar-slides').anythingSlider({
                        easing: "easeInOutExpo",
                        autoPlay: false,
                        delay: 8000,
                        startStopped: false,
                        animationTime: 500,
                        hashTags: false,
                        buildNavigation: false,
                        pauseOnHover: false,
                        startText: "Go",
                        stopText: "Stop"
                    });
                }, 500);

            });


            function proximo() {
                $('#pre-visualizar-slides').data('AnythingSlider').goForward();
            };

            function anterior() {
                $('#pre-visualizar-slides').data('AnythingSlider').goBack();
            };

    </script>
</head>
<body style="background:none #fff !important;">
    <div>

        <div id="PreVisualizar">

	        <p style="color: White; font-weight: bold; text-align: center; position: absolute; left: 50%; margin-left: -130px;">
		        Utilize as setas para navegar no VA
	        </p>

	        <a href="javascript:anterior();" class="anterior" style="float: left; width: 50px; position: absolute; left: 0; top: 50%; margin-top: - 50px; z-index: 999;">
		        <img src="<%=Url.Content("~/") %>images/anterior.gif" alt="Anterior" />
	        </a>


	        <div class="anyContainer" style="position: absolute; left: 50%; margin-left: -512px; top: 50%; margin-top: -384px">

		        <div class="anythingSliderVisualizar" id="pre-visualizar-slides" style="float: left;">
			        <div class="wrapper">
				         <%=ViewData["conteudo"]%>
			        </div>
		        </div>

	        </div>

	        <a href="javascript:proximo();" class="proximo" style="float: left; width: 50px; position: absolute; right: 0; top: 50%; margin-top: - 50px; z-index: 999;">
		        <img src="<%=Url.Content("~/") %>images/proximo.gif" alt="Próximo" />
	        </a>

        </div>
       
    </div>
</body>
</html>
