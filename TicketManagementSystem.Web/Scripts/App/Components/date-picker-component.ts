var datepickerComponent = Vue.extend({
    template: `
        <div class="input-group date">
            <input type="text" class="form-control text-box single-line" v-model="value" v-el:input readonly>
        </div>`,
    props: {
        value: ''
    },
    data: function () {
        return {};
    },
    ready: function () {
        $(this.$els.input).datepicker({
            format: 'yyyy/mm/dd',
            autoclose: true
        });
    }
});

Vue.component('datepicker', datepickerComponent);
