# Wizard
Download the netWizardDemo solution to see how it all works together.
Proper documentation is on my long list of things to do, but this might get you started.

# Setup
Put something like this where you want the form to be:

```
@using (Html.BeginForm("AddNewUser", "Home", FormMethod.Post, new { @class = "wizard-form"}))
{ 
    @Html.AntiForgeryToken();
    @Html.ValidationSummary(true, "", new { @class = "text-danger" })
    <div class="wizard-form-nav btn-group btn-group-justified">
    </div>
    <div class="wizard-form-pages">
        <fieldset name="Create User" data-url="@Url.Action("UserWizard", "MUsers")"></fieldset>
        <fieldset name="Add Applications" data-url="@Url.Action("ApplicationWizard", "MApplications")"></fieldset>
        <fieldset name="Add Addresses" data-url="@Url.Action("AddressWizard", "MAddresses")"></fieldset>
    </div>
    <div class="wizard-form-controls btn-group btn-group-justified">
    </div>
} 
```

Initialize the form with:

```
<script>
    $(document).ready(function () {
        $(".wizard-form").Wizard({
            size: 500, 
            onSubmit: function (e, form) {
                alert(form.serialize());
                //e.preventDefault();
                //alert("I was submitted");
            }
        });
    });
</script>
```

For your form fields, simply create views that return the desired set of fields on each wizard page.  Each fieldset represents a different step in the Wizard.

This plugin also supports dynamically added fields.  Refer to the demo solution for guidance on this:
https://github.com/vtsells/Wizard/tree/master/netWizardDemo