import { Menu } from './menu.class';

export interface IMenuService
{
    getMenu():Menu[];
}

let MENU_SERVICE_ID = {
    IMENUSERVICE: Symbol("IMenuService")
};

export { MENU_SERVICE_ID };