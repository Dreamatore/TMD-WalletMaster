﻿@model IEnumerable<TMD_WalletMaster.Core.Models.Budget>

    @{
    ViewData["Title"] = "Budgets";
}

<h1>Budgets</h1>

    <p>
    <a asp-action="Create">Create New</a>
    </p>
    <table class="table">
    <thead>
    <tr>
    <th>
    @Html.DisplayNameFor(model => model.Name)
    </th>
    <th>
    @Html.DisplayNameFor(model => model.Amount)
    </th>
    <th>
    @Html.DisplayNameFor(model => model.StartDate)
    </th>
    <th>
    @Html.DisplayNameFor(model => model.EndDate)
    </th>
    <th></th>
    </tr>
    </thead>
    <tbody>
    @foreach (var item in Model)
{
    <tr>
        <td>
        @Html.DisplayFor(modelItem => item.Name)
        </td>
        <td>
        @Html.DisplayFor(modelItem => item.Amount)
        </td>
        <td>
        @Html.DisplayFor(modelItem => item.StartDate)
        </td>
        <td>
        @Html.DisplayFor(modelItem => item.EndDate)
        </td>
        <td>
        <a asp-action="Edit" asp-route-id="@item.Id">Edit</a> |
        <a asp-action="Details" asp-route-id="@item.Id">Details</a> |
        <a asp-action="Delete" asp-route-id="@item.Id">Delete</a>
        </td>
        </tr>
}
</tbody>
    </table>