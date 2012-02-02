<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div class="sub-menu">
    <%=Html.ActionLink("Usuários","index", "usuarios") %>
    <%=Html.ActionLink("Grupos/Permissões","index", "grupos") %>
</div>