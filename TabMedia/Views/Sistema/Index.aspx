<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Sistema
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="titulo">
    	<h4></h4>
        <h2>Sistema</h2>
        <h4></h4>
    </div>
    
    <div class="conteudo-form">
        <ul>
            <li style="height: auto;">
                <h2><a href="<%=Url.Action("editar") %>">[Editar]</a></h2>
            </li>
            <li style="height: auto; margin-top: 10px;">
                <b>Nome do Sistema:</b> <%=Util.Configuracao.AppSettings("NomeSistema") %>
                <br /><b>Site Local:</b> <%=Util.Configuracao.AppSettings("SiteUrlLocal")%>
                <br /><b>Site Online:</b> <%=Util.Configuracao.AppSettings("SiteUrlOnline")%>
                <br /><b>Host Online:</b> <%=Util.Configuracao.AppSettings("HostOnline")%>
            </li>
            <li style="height: auto; margin-top: 10px;">
                <h3>Autenticação</h3>
                <b>Endereço da Pagina de Login:</b> <%=Util.Configuracao.AppSettings("UrlLogin")%>
                <br /><b>Endereço Padrão após Logar:</b> <%=Util.Configuracao.AppSettings("UrlDefault")%>
                <br /><b>Endereço da Pagina de Login (Admin):</b> <%=Util.Configuracao.AppSettings("UrlLoginAdmin")%>
                <br /><b>Endereço Padrão após Logar (Admin):</b> <%=Util.Configuracao.AppSettings("UrlDefaultAdmin")%>
            </li>
            <li style="height: auto; margin-top: 10px;">
                <h3>Email</h3>
	            <b>Nome de Envio Padrão:</b> <%=Util.Configuracao.AppSettings("NomeEnvioPadrao")%>
                <br /><b>Email de Envio Padrão:</b> <%=Util.Configuracao.AppSettings("EmailEnvioPadrao")%>
                <br /><b>Smtp Host:</b> <%=Util.Configuracao.AppSettings("SmtpHost")%>
                <br /><b>Utilizar Autenticação:</b> 
                <%
                try
                {
                    if (Convert.ToBoolean(Util.Configuracao.AppSettings("SmtpAutenticado")))
                        Response.Write("Sim");
                    else
                        Response.Write("Não");
                }
                catch{
                    Response.Write("<font color=\"red\">Há um erro neste parâmetro</font>");
                }%>
                <b>Smtp Username:</b> <%=Util.Configuracao.AppSettings("SmtpUsername")%>
                <br /><b>Smtp Password:</b> <%=Util.Configuracao.AppSettings("SmtpPassword")%>
                <br />
                <br /><b>Nome de Contato Padrão:</b> <%=Util.Configuracao.AppSettings("NomeContatoPadrao")%>
                <br /><b>Email de Contato Padrão:</b> <%=Util.Configuracao.AppSettings("EmailContatoPadrao")%>
                <br /><b>Email do Desenvolvedor:</b> <%=Util.Configuracao.AppSettings("EmailDesenvolvedor")%>
            </li>
            <li style="height: auto; margin-top: 10px;">
                <h3>Analytics</h3>
                <b>Analytics id:</b> <%=Util.Configuracao.AppSettings("AnalyticsId")%>
            </li>
            <li style="height: auto; margin-top: 10px;">
                <h3>Addthis</h3>
		        <b>Utilizar Addthis:</b> 
		        <%
                try
                {
                    if (Convert.ToBoolean(Util.Configuracao.AppSettings("AddthisEnabled")))
                        Response.Write("Sim");
                    else
                        Response.Write("Não");
                }
                catch{
                    Response.Write("<font color=\"red\">Há um erro neste parâmetro</font>");
                }%>
                <br /><b>Username:</b> <%=Util.Configuracao.AppSettings("AddthisUsername")%>
            </li>
            <li style="height: auto; margin-top: 10px;">
                <h3>Validação dos Formulários - Validation Engine</h3>
        
                <b>Cor Fundo:</b> <%=Util.Configuracao.AppSettings("VECorFundo")%>
                <br /><b>Cor Fonte:</b> <%=Util.Configuracao.AppSettings("VECorFonte")%>
                <br /><b>Largura Box:</b> <%=Util.Configuracao.AppSettings("VELargura")%>
                <br /><b>Tamanho Sombra:</b> <%=Util.Configuracao.AppSettings("VETamanhoSombra")%>
                <br /><b>Curvatura Box:</b> <%=Util.Configuracao.AppSettings("VETamanhoCurvaBorda")%>
            </li>
        </ul>
    </p>

</asp:Content>