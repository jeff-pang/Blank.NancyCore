import { Container } from 'inversify';
import 'reflect-metadata';

//menu service config
import { IMenuService, MENU_SERVICE_ID } from './menu/menu.service';
import { MockMenuService } from './menu/menu.service.mock';

let container: Container = new Container();
container.bind<IMenuService>(MENU_SERVICE_ID.IMENUSERVICE).to(MockMenuService);


export default container;