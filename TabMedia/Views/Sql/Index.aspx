<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Data.DataTable>" %>
<%@ Import Namespace="System.Data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <script type="text/javascript">

        function tabelas() {
            $('#sql').val('select * from information_schema.tables order by table_name');
        }

        function addColuna() {
            $('#sql').val('ALTER TABLE [dbo].[tabela] ADD coluna int NULL');
        }
        
        function conexao() {

            alert('<%=ConfigurationManager.ConnectionStrings["MakroCestasConnectionString"]%>');
            
        }


        $(function() {

            $('.resizable').resizable();

        });
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="titulo">
        <h2>Gerenciador SQL</h2>
    </div>
    
    <div class="top-tabela-cinza-cadastro conteudo-baixo" style="padding: 10px; min-height: 400px;">
        
        <div class="conteudo-secundario-container corner">
        
            <h2>SQL</h2>
            
            <a href="javascript:tabelas();">Ver Tabelas</a> | <a href="javascript:addColuna();">Add Coluna</a> | <a href="javascript:conexao();">Conexão</a>

            
            <form action="" method="post">
            
                <textarea id="sql" class="resizable" name="sql" style="width: 500px; height: 200px;"><%=Request.Form["sql"] %></textarea>
                
                <br /><br />
                <input type="submit" style="width: 100px" value="Enviar" />
            
            </form>
            
            <%=ViewData["result"]%>
            
            <% if (Model != null)
               { %>
            
            <table cellspacing="3" cellpadding="0" border="0" class="tabela tablesorter"><colgroup>
                <thead>
                <tr>
                    <% foreach (DataColumn coluna in Model.Columns)
                       { %>
                        <th>
                            <%=coluna.ColumnName%>
                        </th>
                    <%} %>
                </tr>
                </thead>
                <tbody>
                    <% foreach (DataRow row in Model.Rows)
                       { %>
                    <tr class="even">
                        
                        <% foreach (DataColumn coluna in Model.Columns)
                           { %>
                            <td>
                                <%=row[coluna.ColumnName]%>
                            </td>
                        <%} %>
                    </tr>
                    <%} %>
                </tbody>
            </table>
            
            <%} %>
            
        </div>
    </div>

</asp:Content>

