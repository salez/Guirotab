<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Models.Erro>" %>

<p>
    <b>Erro #<%= Html.Encode(Model.Id) %></b>
</p>
<p>
    <b>Data: </b><%= Html.Encode(Model.DataInclusao.Formata(Util.Data.FormatoData.Completo))%>
</p>
<p>
    <b>Página: </b><%= Html.Encode(Model.Pagina)%>
</p>
<p>
    <b>Página Anterior: </b><%= Html.Encode(Model.PaginaAnterior)%>
</p>    
<p>
    <b>Página: </b><%= Html.Encode(Model.Pagina)%>
</p>    
<p>
    <b>Request Host: </b><%= Html.Encode(Model.RequestHost)%>
</p>
<p>
    <b>Host Name: </b><%= Html.Encode(Model.HostName)%>
</p>
<p>
    <b>User Agent: </b><%= Html.Encode(Model.UserAgent)%>
</p>
<p>
    <b>Usuário: </b><%= Model.InfoUsuario%>
</p>
<p>
    <%=Model.ErroMsg.Nl2br()%>
</p>