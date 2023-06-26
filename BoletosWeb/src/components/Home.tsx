import "./Home.style.css"
import Sidebar from "./subcomponents/sidebar";
import React, { useState } from 'react';
import { categorias } from '../api/categorias.tsx'
import { BoletosComprados } from '../../models/Boletos.tsx'
import { eventos } from '../api/eventos.tsx'
import SearchBar from "./subcomponents/searchbar.tsx";
import Body from "./subcomponents/body.tsx"
import { Modal, ModalBody } from "reactstrap"

function Home() {

    const [categories, setCategories] = useState<CategoriaEventos[]>([]);
    const [events, setEventos] = useState<Eventos[]>([]);
    const [allEvents, setAllEventos] = useState<Eventos[]>([]);


    const [listaBoletos, setListaBoletos] = useState<BoletosComprados[]>([]);

    const handleBoletosAgregados = (boletos: BoletosComprados[]) => {
        setListaBoletos((prevLista) => [...prevLista, ...boletos]);
    };



    const [categoryId, setCategory] = React.useState<number>(0)
    const handleClick = (numero: number) => {
        setCategory(numero);
    };

    const handleSearch = (input: string) => {

        setEventos(allEvents.filter((evento) => {
            return evento.nombreEvento.includes(input) || evento.descripcion.includes(input)
        }));
    };

    const [dialogVisible, setDialogVisible] = useState(false);

    const onCompraCheck = () => {
        setDialogVisible(true);
    };

    const closeDialog = () => {
        setDialogVisible(false);
    };


    React.useEffect(() => {
        categorias
            .getAll().then((r) => {
                setCategories(r.data);
            })
            .catch((e) => {
                console.error(e);
            });
        eventos
            .getAllByCategory(categoryId).then((r) => {
                setEventos(r.data);
            })
            .catch((e) => {
                console.error(e);
            });
        eventos
            .getAll().then((r) => {
                setAllEventos(r.data);
            })
            .catch((e) => {
                console.error(e);
            });
    }, [categoryId]);

    return (
        <>
            <article className="article-header">
                <img className="profile-icon" src="/src/components/resources/profileIcon.png"></img>
                <header className="header">
                    <img src="/src/components/resources/BoletosPF_Icon.png" className="header-logo" />
                    <h1 className="header-title">Ticket tracker</h1>
                    <h5 className="header-subtitle">¡Compra tus boletos sin límites, sin estrés!</h5>
                </header>
            </article>
            <section>
                <div className="page-body">
                    <Sidebar lista={categories} onCategoryClick={handleClick} />
                    <div className="body-col">
                        <SearchBar OnSearch={handleSearch} OnCheckCompraClick={onCompraCheck} />
                        <Body lista={events} OnBoletosAgregados={handleBoletosAgregados} />
                    </div>
                </div>
            </section>
            {dialogVisible && <CarritoDialog onClose={closeDialog} opened={dialogVisible} listaCompra={listaBoletos} />}
        </>
    );
}

export default Home


function CarritoDialog({ onClose, opened, listaCompra}: { onClose: () => void, opened: boolean, listaCompra: BoletosComprados[]}) {

    const [email, setEmail] = React.useState<string>("")
    const emailRegex = /^(?=.{1,254}$)(?=.{1,64}@)[-!#$%&'*+/0-9=?A-Z^_`a-z{|}~]+(\.[-!#$%&'*+/0-9=?A-Z^_`a-z{|}~]+)*@[A-Za-z0-9]([A-Za-z0-9-]{0,61}[A-Za-z0-9])?(\.[A-Za-z0-9]([A-Za-z0-9-]{0,61}[A-Za-z0-9])?)*$/;
    const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setEmail(event.target.value);
    };

    const onCerrarClick = () => {
        onClose()
    };

    return (
        <>
            <Modal isOpen={opened} className="scrim">
                <div className="closeRow">
                    <label onClick={onCerrarClick} className="cerrarBTN">X</label>
                </div>
                <ModalBody className="dialog" >
                    <h2 className="title">Carrito de compras</h2>
                    {/* Correo div */}
                    <label>Correo: </label>
                    <div className="correo-field" >
                        <input className="correo-input" value={email} onChange={handleInputChange} type="text"></input>
                    </div>

                    {/* Lista de detalles div */}
                    <div className="detalles-div" >

                    </div>
                    {/* botones div */}
                    <div className="buttons-panel" >
                        <button onClick={onClose} className="bttn">Cancelar</button>
                        <button className="bttn" >Comprar</button>
                    </div>
                </ModalBody>
            </Modal>
        </>
    );
}
