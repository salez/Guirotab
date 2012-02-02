<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<MakroCestas.Areas.Admin.Models.Oferta>" %>
<%@ Import Namespace="MakroCestas.Areas.Admin.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Alterar
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <script type="text/javascript">

        $(function() {

            $('.data').datepicker({ dateFormat: 'dd/mm/yy' });


            $('#gravar').click(function() {

                $('#frmOferta').submit();
            });

        });
        
    </script>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="titulo">
        <h2>Alterar</h2>
    </div>
    
    <style>
        label{
            display: block;
            background: none;
            clear: both;
        }
        li{
            display: block;
            background: none;
            clear: both;
        }
    </style>
    
    <div class="top-tabela-cinza-cadastro conteudo-baixo" style="padding: 10px; min-height: 400px;">
        
        <div class="conteudo-secundario-container corner">
        
            <font color="red" style="display: block"><%=ViewData["erro"] %></font>
            
    	    <form id="frmOferta" method="post" action="">
                <div style="float: left;">
        	        <label for="DataInicio">Data Inicial</label>
        	         <%=Html.TextBox("DataInicio", Model.DataInicio.Formata(Util.Data.FormatoData.DiaMesAno), new { @class = "data validate[required]" })%>
                </div>
                <div style="float: left; margin-left: 10px;">
                    <label for="DataFim">Data Final</label>
                    <%=Html.TextBox("DataFim", Model.DataFim.Formata(Util.Data.FormatoData.DiaMesAno), new { @class = "data validate[required]" })%>
               </div>
               
               <div class="sub-menu" style="clear: both;">
                    <div class="publicacao">
                        <a href="#" id="gravar">Gravar</a>
                    </div>
               </div>
               
            </form>
            
            <div id="ofertasCestasImportadas" style="clear: both"></div>
            
        </div>
        
    </div>

</asp:Content>


