import Vue from 'vue';
import Component from 'vue-class-component';
import MenuComponent from './menu.component';

var vmMenu = new Vue({
    el:"mainmenu",
    render: h=> h(MenuComponent)
});