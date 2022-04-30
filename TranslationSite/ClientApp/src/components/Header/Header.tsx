import { Navbar, Container } from 'react-bootstrap';
import logoImage from './../../assets/img/logo.svg'
import styles from './Header.module.scss'

const Header = () => {
    return (
        <Navbar bg="light" expand="lg">
        <Container>
            <Navbar.Brand href="/"><div className={styles.logo}><img src={logoImage} alt="" /></div></Navbar.Brand>
        </Container>
        </Navbar>
    );
  }
  
  export default Header;