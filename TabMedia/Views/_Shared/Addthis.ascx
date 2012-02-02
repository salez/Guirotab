<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<%if(Convert.ToBoolean(Util.Configuracao.AppSettings("AddthisEnabled"))){ %>

<!--Habilita função de Clickbacks no site-->
<script type="text/javascript">
    var addthis_config =
    {
        data_track_linkback: true
    }
</script>

<script type="text/javascript" src="http://s7.addthis.com/js/250/addthis_widget.js#username=<%=Util.Configuracao.AppSettings("AddthisUsername") %>"></script>

<div class="divAddThis">
    <div class="addthis_toolbox addthis_default_style addthis_32x32_style">
	    <ul>
		    <li><a class="addthis_button_twitter"></a></li>
		    <li><a class="addthis_button_facebook"></a></li>
		    <li><a class="addthis_button_orkut"></a></li>
		    <li><a class="addthis_button_myspace"></a></li>
		    <li><a class="addthis_button_google"></a></li>
		    <li><a class="addthis_button_email"></a></li>
	    </ul>
    </div>
</div>

<% } %>