import Header from './components/Header/Header';
import Translate from './components/Translate/Translate';
import './App.css';

import { Provider } from 'react-redux';
import { store } from './store/store';

function App() {
  return (
    <Provider store={store}>
      <Header />
      <Translate />
    </Provider>
  );
}

export default App;
