<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MakroCestas.Areas.Admin.Models.Oferta>" %>
<%@ Import Namespace="MakroCestas.Areas.Admin.Models" %>

<%

    LojaRepository lojaRepository = new LojaRepository();
    
    //seleciona as lojas onde existem cestas na oferta.
    var lojas = lojaRepository.GetLojas().Where(l => l.OfertaCestas.Any(c => c.Oferta.Id == Model.Id)).OrderBy(l => l.Numero);

    var lojasNaoImportadas = lojaRepository.GetLojas().Where(l => !l.OfertaCestas.Any(c => c.Oferta.Id == Model.Id)).OrderBy(l => l.Numero);
%>

<% if(lojasNaoImportadas.Count() > 0) { %>
    
    <div id="atencao" style="border: 1px solid red; margin: 0 auto; width: 400px; padding: 10px;">
        <p><br />
            <center>
                <img src="<%=Url.Content("~/images/ico-atencao.gif") %>" />
                <br /><br />
                <span class="msg" style="color: Red">
                    Atenção! há lojas presentes no sistema que não foram importadas na planilha:
                 </span>
            </center>
            <br /><br />
            
            <table>
                <thead>
                    <tr>
                        <th>Numero</th>
                        <th>Loja</th>
                    </tr>
                </thead>
                <tbody>
                <% foreach (var loja in lojasNaoImportadas)
                  { %>
                    
                    <tr>
                        <td>
                            <%=loja.Numero %>
                        </td>
                        <td>
                            <%=loja.Nome %>
                        </td>
                    </tr>
                
                <% } %>
                </tbody>
            </table>
            
        </p>
    </div>
    

<% } %>

<table width="996" border="0" cellpadding="0" cellspacing="3" id="tabela" class="tabela tablesorter">
    <thead>
        <tr>
            <th style="text-align: center">Número</th>
            <th style="text-align: center">Loja</th>
            <th style="text-align: center">
                Tipo 1
            </th>
            <th style="text-align: center">
                Tipo 2
            </th>
            <th style="text-align: center">
                Tipo 3
            </th>
            <th style="text-align: center">
                Tipo E
            </th>
        </tr>
        </thead>
    <tbody>
    
<%foreach(var loja in lojas) {
      var tipo1 = Model.OfertaCestas.FirstOrDefault(c => c.Loja.Id == loja.Id && c.Tipo == '1');
      var tipo2 = Model.OfertaCestas.FirstOrDefault(c => c.Loja.Id == loja.Id && c.Tipo == '2');
      var tipo3 = Model.OfertaCestas.FirstOrDefault(c => c.Loja.Id == loja.Id && c.Tipo == '3');
      var tipoE = Model.OfertaCestas.FirstOrDefault(c => c.Loja.Id == loja.Id && c.Tipo == 'E');
      %>

        <tr>
            <td style="text-align: center">
                <%= Html.Encode(loja.Numero)%>
            </td>
            <td>
                <%= Html.Encode(loja.Nome) %>
            </td>
            <td style="text-align: right">
                <%= (tipo1 != null) ? String.Format("{0:F2}", tipo1.Preco) : ""%>
            </td>
            <td style="text-align: right">
                <%= (tipo2 != null) ? String.Format("{0:F2}", tipo2.Preco) : ""%>
            </td>
            <td style="text-align: right">
                <%= (tipo3 != null) ? String.Format("{0:F2}", tipo3.Preco) : ""%>
            </td>
            <td style="text-align: right">
                <%= (tipoE != null) ? String.Format("{0:F2}", tipoE.Preco) : ""%>
            </td>
        </tr>

<%} %>

    </tbody>
</table>