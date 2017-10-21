import {Menu} from './menu.class';
import { IMenuService } from './menu.service';
import { injectable } from 'inversify';


const MENU : Menu[] = [
    { name:"Home", url:"#" , icon:"entypo-chart-bar"},
    { name:"Menu Item 2", url:"#", icon:"entypo-chart-bar" },
    { name:"Menu Item 3", url:"#", icon:"entypo-chart-bar" }
]

@injectable()
export class MockMenuService implements IMenuService{

    getMenu():Menu[]{
        return MENU;
    }
}