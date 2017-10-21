import Vue from 'vue';
import Component from 'vue-class-component';
import {Menu} from './menu.class';
import { IMenuService, MENU_SERVICE_ID } from './menu.service';
import container from '../dependencyConfig';

@Component({
    template:require('./tpl/menu.html')
})

export default class MenuComponent extends Vue
{
    _menuService: IMenuService;

    menus : Menu[];

    created(){
        this._menuService = container.get<IMenuService>(MENU_SERVICE_ID.IMENUSERVICE);
    }
    mounted(){
        
        this.menus=this._menuService.getMenu();
    }

    data () {
        return {
          // Will be reactive
          menus : Array<Menu>()
        }
    }
}