<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Models.Email>" %>

<p>
    <b>Email #<%= Html.Encode(Model.Id) %></b>
</p>
<p>
    <b>Data: </b><%= Html.Encode(Model.DataInclusao.Formata(Util.Data.FormatoData.Completo))%>
</p>
<p>
    <b>Assunto: </b><%= Html.Encode(Model.Assunto)%>
</p>
<p>
    <b>Remetente: </b><%= Html.Encode(Model.De)%> (<%=Html.Encode(Model.DeEmail) %>)
</p>    
<p>
    <b>Destinatários: </b><br />
    <%foreach (var destinatario in Model.EmailDestinatarios.Where(e => e.Tipo == (Char)Models.Email.EnumTipo.Destinatario))
      { %>
      
        <br /><%= Html.Encode(destinatario.Email)%>
    
    <%} %>
</p> 
<p>
    <b>Destinatários (cópia): </b><br />
    <%foreach (var destinatario in Model.EmailDestinatarios.Where(e => e.Tipo == (Char)Models.Email.EnumTipo.Copia))
      { %>
      
        <br /><%= Html.Encode(destinatario.Email)%>
    
    <%} %>
</p>    
<p>
    <b>Destinatários (cópia oculta): </b><br />
    <%foreach (var destinatario in Model.EmailDestinatarios.Where(e => e.Tipo == (Char)Models.Email.EnumTipo.CopiaOculta))
      { %>
      
        <br /><%= Html.Encode(destinatario.Email)%>
    
    <%} %>
</p>       
<p>
    <b>Encode: </b><%= Html.Encode(Model.Encode)%>
</p>
<p>
    <b>Corpo HTML: </b>
    
    <%= (Model.CorpoHtml.Value)?"Sim":"Não"%>
</p>
<p>
    <b>Host: </b><%= Html.Encode(Model.Host)%>
</p>
<p>
    <b>Porta: </b><%= Html.Encode(Model.Porta)%>
</p>
<p>
    <b>Smtp Autenticado: </b><%= (Model.SmtpAutenticado.Value)?"Sim":"Não"%>
</p>
<p>
    <b>Smtp Usuário: </b><%= Html.Encode(Model.SmtpUsuario)%>
</p>
<p>
    <b>Smtp Senha: </b><%= Html.Encode(Model.SmtpSenha)%>
</p>
<p>
    <b>SSL Habilitado: </b><%= (Model.SslHabilitado.Value)?"Sim":"Não"%>
</p>
<p>
    <b>Corpo: </b>
    <div style="overflow: auto; width: 700px; border: 2px solid black;">
            <%= Model.Corpo%>
    </div>
</p>