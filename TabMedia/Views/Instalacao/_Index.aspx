<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Home
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="titulo">
        <h2>Instalação</h2>
    </div>
    
    <div class="conteudo-baixo vas-visualizar">
        
        <h2 style="margin-top: 0;">Habilitar Aplicativo</h2>

        Clique no link abaixo a partir do seu iPad para instalar o aplicativo
        
        <br /><br /><a class="botao" href="<%=Url.Action("Instalar") %>" target="_blank">Instalar</a>

    </div>

</asp:Content>